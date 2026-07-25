using TikTokExplode.Domain.Entities;
using TikTokExplode.Domain.Interfaces;
using TikTokExplode.Infrastructure.Extraction;
using TikTokExplode.Infrastructure.Http;
using TikTokExplode.Infrastructure.Url;

namespace TikTokExplode.Infrastructure.Repositories;

public sealed class VideoRepository : IVideoRepository
{
    private readonly ITikTokApiClient _apiClient;
    private readonly IVideoExtractor _videoExtractor;
    private readonly UrlHandler _urlHandler;

    public VideoRepository(
        ITikTokApiClient apiClient,
        IVideoExtractor videoExtractor,
        UrlHandler urlHandler)
    {
        _apiClient = apiClient;
        _videoExtractor = videoExtractor;
        _urlHandler = urlHandler;
    }

    public async Task<Video> GetByUrlAsync(string url, CancellationToken ct = default)
    {
        var fullUrl = await _urlHandler.GetFullUrlAsync(url, ct);
        var jsonResponse = await _apiClient.GetApiResponseAsync(fullUrl, ct);
        return _videoExtractor.ExtractVideo(jsonResponse);
    }
}
