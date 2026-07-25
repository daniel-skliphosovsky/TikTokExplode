using System.Net;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using TikTokExplode.Domain.Exceptions;
using TikTokExplode.Infrastructure.Configuration;

namespace TikTokExplode.Infrastructure.Http;

public sealed class TikTokApiClient : ITikTokApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HeadersProvider _headersProvider;
    private readonly TikTokApiOptions _options;

    public TikTokApiClient(
        IHttpClientFactory httpClientFactory,
        HeadersProvider headersProvider,
        IOptions<TikTokApiOptions> options)
    {
        _httpClientFactory = httpClientFactory;
        _headersProvider = headersProvider;
        _options = options.Value;
    }

    public async Task<string> GetApiResponseAsync(string url, CancellationToken ct = default)
    {
        var client = _httpClientFactory.CreateClient("TikTokApi");
        client.Timeout = TimeSpan.FromSeconds(_options.TimeoutSeconds);

        var lastException = default(Exception);

        for (int attempt = 0; attempt < _options.MaxRetries; attempt++)
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, url);
                _headersProvider.ApplyHeaders(request.Headers);

                using var response = await client.SendAsync(request, ct);

                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    if (attempt < _options.MaxRetries - 1)
                    {
                        await Task.Delay(_options.RetryDelayMilliseconds * (attempt + 1), ct);
                        continue;
                    }
                    throw new ApiException("Forbidden by TikTok API", 403, string.Empty);
                }

                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync(ct);
            }
            catch (Exception ex) when (ex is TaskCanceledException or HttpRequestException)
            {
                lastException = ex;
                if (attempt < _options.MaxRetries - 1)
                {
                    await Task.Delay(_options.RetryDelayMilliseconds * (attempt + 1), ct);
                }
            }
        }

        throw new ApiException(
            "Failed to get API response after retries",
            0,
            lastException?.Message ?? string.Empty,
            lastException ?? new Exception("Unknown error"));
    }

    public async Task<Stream> GetStreamAsync(string url, CancellationToken ct = default)
    {
        var client = _httpClientFactory.CreateClient("TikTokApi");
        client.Timeout = TimeSpan.FromSeconds(_options.TimeoutSeconds);

        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        _headersProvider.ApplyHeaders(request.Headers);

        var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStreamAsync(ct);
    }
}
