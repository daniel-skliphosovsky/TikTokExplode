namespace TikTokExplode.Infrastructure.Http;

public interface ITikTokApiClient
{
    Task<string> GetApiResponseAsync(string url, CancellationToken ct = default);
    Task<Stream> GetStreamAsync(string url, CancellationToken ct = default);
}
