using TikTokExplode.Domain.Exceptions;

namespace TikTokExplode.Infrastructure.Url;

public sealed class UrlHandler
{
    private readonly IHttpClientFactory _httpClientFactory;

    public UrlHandler(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// Follows redirects to resolve a short TikTok URL to its full form.
    /// </summary>
    /// <param name="url">The TikTok URL to resolve.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The resolved full URL.</returns>
    /// <exception cref="ApiException">Thrown when URL resolution fails.</exception>
    public async Task<string> GetFullUrlAsync(string url, CancellationToken ct = default)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("TikTokApi");
            client.Timeout = TimeSpan.FromSeconds(10);

            using var request = new HttpRequestMessage(HttpMethod.Head, url);
            using var response = await client.SendAsync(request, ct);

            if (response.StatusCode == System.Net.HttpStatusCode.Found ||
                response.StatusCode == System.Net.HttpStatusCode.Moved ||
                response.StatusCode == System.Net.HttpStatusCode.MovedPermanently)
            {
                var redirectUrl = response.Headers.Location?.ToString();
                if (!string.IsNullOrEmpty(redirectUrl))
                {
                    if (redirectUrl.StartsWith("/"))
                    {
                        var baseUri = new Uri(url);
                        redirectUrl = $"{baseUri.Scheme}://{baseUri.Host}{redirectUrl}";
                    }
                    return redirectUrl;
                }
            }

            return url;
        }
        catch (Exception ex)
        {
            throw new ApiException("Failed to resolve URL", 0, ex.Message, ex);
        }
    }
}
