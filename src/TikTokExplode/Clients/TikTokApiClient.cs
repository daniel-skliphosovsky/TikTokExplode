using TikTokExplode.Exceptions;

namespace TikTokExplode;

/// <summary>
/// Internal HTTP client that talks to the TikTok API and media servers.
/// Applies randomized headers and retry-with-backoff on every request.
/// </summary>
internal sealed class TikTokApiClient
{
    private readonly HttpClient _httpClient;
    private readonly TikTokClientOptions _options;
    private readonly HttpRetryHandler _retryHandler;

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

        using HttpResponseMessage response = await SendWithRetryAsync(apiUrl, ct).ConfigureAwait(false);
        string content = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
            throw new ApiException(
                $"TikTok API request failed with status code {(int)response.StatusCode} ({response.StatusCode}).",
                (int)response.StatusCode,
                content);

        return content;
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
}
