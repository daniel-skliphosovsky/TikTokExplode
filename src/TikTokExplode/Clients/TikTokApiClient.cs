using TikTokExplode.Exceptions;

namespace TikTokExplode;

/// <summary>
/// Internal HTTP client that talks to the TikTok API and media servers.
/// Applies randomized User-Agents and retries the single api22 endpoint with a
/// fresh User-Agent per attempt until a JSON response arrives (v1.3.1 mechanism).
/// </summary>
internal sealed class TikTokApiClient
{
    private readonly HttpClient _httpClient;
    private readonly TikTokClientOptions _options;
    private readonly HttpRetryHandler _retryHandler;

    // TikTok rate-limits api22 aggressively (the normal response is a
    // "ratelimit triggered" 429), so a single attempt rarely succeeds.
    // v1.3.1 retried with a fresh random User-Agent until a JSON response
    // arrived; this constant bounds that loop while keeping it effective.
    private const int MaxApiAttempts = 30;

    public TikTokApiClient(HttpClient httpClient, TikTokClientOptions options)
    {
        _httpClient = httpClient;
        _options = options;
        _retryHandler = new HttpRetryHandler(options.RetryCount, options.RetryBaseDelayMs);
    }

    public async Task<string> GetApiResponseAsync(string url, CancellationToken ct = default)
    {
        string awemeId = UrlHelper.ExtractAwemeId(url)
            ?? throw new ValidationException("Failed to extract aweme ID from URL.");

        string apiUrl = _options.ApiUrl.Replace("{awemeId}", awemeId);

        // v1.3.1 mechanism: retry the single endpoint with a fresh random
        // User-Agent per attempt until a JSON response arrives.
        for (int attempt = 0; attempt < MaxApiAttempts; attempt++)
        {
            ct.ThrowIfCancellationRequested();

            using HttpResponseMessage response = await _httpClient
                .SendAsync(CreateRequest(apiUrl), HttpCompletionOption.ResponseContentRead, ct)
                .ConfigureAwait(false);

            string content = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);

            if (response.IsSuccessStatusCode && LooksLikeJson(content))
                return content;
        }

        throw new ApiException(
            "TikTok API request failed after several attempts. Try again in a minute.",
            0,
            string.Empty);
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

    private HttpRequestMessage CreateRequest(string url)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.UserAgent.ParseAdd(_options.UserAgents[Random.Shared.Next(_options.UserAgents.Length)]);
        return request;
    }
}
