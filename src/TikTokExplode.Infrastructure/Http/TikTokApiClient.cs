using System.Net;
using Microsoft.Extensions.Options;
using TikTokExplode.Domain.Exceptions;
using TikTokExplode.Infrastructure.Configuration;

namespace TikTokExplode.Infrastructure.Http;

public sealed class TikTokApiClient : ITikTokApiClient
{
    private readonly HttpClient _httpClient;

    public TikTokApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetApiResponseAsync(string url, CancellationToken ct = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        using var response = await _httpClient.SendAsync(request, ct);

        if (response.StatusCode == HttpStatusCode.Forbidden)
            throw new ApiException("Forbidden by TikTok API", 403, string.Empty);

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(ct);
    }

    public async Task<Stream> GetStreamAsync(string url, CancellationToken ct = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStreamAsync(ct);
    }
}
