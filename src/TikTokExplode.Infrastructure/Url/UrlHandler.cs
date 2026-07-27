using System.Net;
using TikTokExplode.Domain.Exceptions;
using TikTokExplode.Infrastructure.Http;

namespace TikTokExplode.Infrastructure.Url;

public sealed class UrlHandler
{
    private readonly ITikTokApiClient _apiClient;
    private readonly IHttpClientFactory _httpClientFactory;

    public UrlHandler(ITikTokApiClient apiClient, IHttpClientFactory httpClientFactory)
    {
        _apiClient = apiClient;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> GetFullUrlAsync(string url, CancellationToken ct = default)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("TikTokApi");
            using var request = new HttpRequestMessage(HttpMethod.Head, url);
            request.Headers.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");

            using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct);

            if (response.StatusCode == HttpStatusCode.Found ||
                response.StatusCode == HttpStatusCode.MovedPermanently ||
                response.StatusCode == HttpStatusCode.Redirect)
            {
                var redirectUrl = response.Headers.Location?.ToString();
                if (!string.IsNullOrEmpty(redirectUrl))
                    return redirectUrl;
            }

            response.EnsureSuccessStatusCode();
            return url;
        }
        catch (Exception ex)
        {
            throw new ApiException("Failed to resolve URL", 0, ex.Message, ex);
        }
    }
}
