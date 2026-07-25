using TikTokExplode.Domain.Exceptions;

namespace TikTokExplode.Infrastructure.Url;

public sealed class UrlHandler
{
    public async Task<string> GetFullUrlAsync(string url, CancellationToken ct = default)
    {
        try
        {
            using var handler = new HttpClientHandler
            {
                AllowAutoRedirect = false,
                MaxAutomaticRedirections = 5
            };
            
            using var client = new HttpClient(handler);
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
