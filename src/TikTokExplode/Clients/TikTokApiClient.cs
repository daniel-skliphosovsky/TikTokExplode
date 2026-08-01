using System.Net;
using TikTokExplode.Exceptions;

namespace TikTokExplode;

/// <summary>
/// Internal HTTP client that talks to the TikTok API and media servers.
/// Applies randomized headers, retry-with-backoff on every request and a
/// minimum interval between API calls to avoid triggering rate limiting.
/// </summary>
internal sealed class TikTokApiClient
{
    private readonly HttpClient _httpClient;
    private readonly TikTokClientOptions _options;
    private readonly HttpRetryHandler _retryHandler;
    private readonly SemaphoreSlim _throttle = new(1, 1);
    private long _nextAllowedAtTicks;

    public TikTokApiClient(HttpClient httpClient, TikTokClientOptions options)
    {
        _httpClient = httpClient;
        _options = options;
        _retryHandler = new HttpRetryHandler(options.RetryCount, options.RetryBaseDelayMs);
    }

    public async Task<string> GetApiResponseAsync(string url, CancellationToken ct = default)
    {
        await ThrottleAsync(ct).ConfigureAwait(false);

        string awemeId = UrlHelper.ExtractAwemeId(url)
            ?? throw new ValidationException("Failed to extract aweme ID from URL.");

        string apiUrl = _options.ApiUrl.Replace("{awemeId}", awemeId);

        bool networkError = false;

        foreach (string? host in GetHosts())
        {
            string candidate = host is null
                ? apiUrl
                : RebuildForHost(apiUrl, host);

            try
            {
                using HttpResponseMessage response = await SendWithRetryAsync(candidate, ct).ConfigureAwait(false);
                string content = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.TooManyRequests)
                        continue; // try the next host

                    string message = response.StatusCode == HttpStatusCode.TooManyRequests
                        ? "TikTok is rate limiting. Try again in a minute."
                        : $"TikTok API request failed with status code {(int)response.StatusCode} ({response.StatusCode}).";

                    throw new ApiException(message, (int)response.StatusCode, content);
                }

                // A 200 with a non-JSON body is a captcha/HTML page from TikTok,
                // not a real response; fall through to the next host.
                if (LooksLikeJson(content))
                    return content;
            }
            catch (HttpRequestException)
            {
                // Network-level failure on this host (DNS, connection refused,
                // timeout) after the retry handler gave up; try the next mirror
                // instead of failing the whole call.
                networkError = true;
            }
        }

        throw new ApiException(
            networkError
                ? "TikTok is unreachable. Check your internet connection or try again later."
                : "TikTok API request failed on all hosts.",
            0,
            string.Empty);
    }

    /// <summary>
    /// Hosts to try: the primary host from the configured url first, then the
    /// configured mirror hosts (deduplicated).
    /// </summary>
    private IEnumerable<string?> GetHosts()
    {
        yield return null; // primary, uses ApiUrl as-is

        string primaryHost = new Uri(_options.ApiUrl).Host;
        foreach (string host in _options.MirrorHosts)
        {
            if (!string.Equals(host, primaryHost, StringComparison.OrdinalIgnoreCase))
                yield return host;
        }
    }

    /// <summary>
    /// Replaces the host of the primary api url with the given mirror host,
    /// keeping the path and query (including the resolved aweme id) intact.
    /// </summary>
    private static string RebuildForHost(string apiUrl, string host)
    {
        var uri = new Uri(apiUrl);
        var builder = new UriBuilder(uri) { Host = host };
        return builder.Uri.AbsoluteUri;
    }

    private static bool LooksLikeJson(string content)
    {
        return content.TrimStart().StartsWith('{');
    }

    /// <summary>
    /// Opens a stream to a media file. The caller owns the returned response.
    /// </summary>
    public async Task<HttpResponseMessage> GetFileResponseAsync(string url, CancellationToken ct = default)
    {
        HttpResponseMessage response = await _retryHandler
            .SendAsync(_httpClient, () => CreateRequest(url), HttpCompletionOption.ResponseHeadersRead, ct)
            .ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            response.Dispose();
            throw new ApiException(
                $"File download request failed with status code {(int)response.StatusCode} ({response.StatusCode}).",
                (int)response.StatusCode,
                string.Empty);
        }

        return response;
    }

    private async Task<HttpResponseMessage> SendWithRetryAsync(string url, CancellationToken ct)
        => await _retryHandler
            .SendAsync(_httpClient, () => CreateRequest(url), HttpCompletionOption.ResponseContentRead, ct)
            .ConfigureAwait(false);

    private HttpRequestMessage CreateRequest(string url)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.UserAgent.ParseAdd(_options.UserAgents[Random.Shared.Next(_options.UserAgents.Length)]);
        request.Headers.Accept.ParseAdd("application/json, text/plain, */*");
        request.Headers.AcceptLanguage.ParseAdd("en-US,en;q=0.9");
        return request;
    }

    /// <summary>
    /// Serializes API calls so that consecutive requests are at least
    /// MinRequestIntervalMs apart, avoiding burst-triggered rate limiting.
    /// </summary>
    private async Task ThrottleAsync(CancellationToken ct)
    {
        await _throttle.WaitAsync(ct).ConfigureAwait(false);
        try
        {
            long now = DateTime.UtcNow.Ticks;
            long remainingMs = Math.Max(0, (Interlocked.Read(ref _nextAllowedAtTicks) - now) / TimeSpan.TicksPerMillisecond);

            if (remainingMs > 0)
                await Task.Delay(TimeSpan.FromMilliseconds(remainingMs), ct).ConfigureAwait(false);

            Interlocked.Exchange(ref _nextAllowedAtTicks, DateTime.UtcNow.AddMilliseconds(_options.MinRequestIntervalMs).Ticks);
        }
        finally
        {
            _throttle.Release();
        }
    }
}
