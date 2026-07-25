using TikTokExplode.Domain.Entities;
using TikTokExplode.Domain.Interfaces;
using TikTokExplode.Infrastructure.Extraction;
using TikTokExplode.Infrastructure.Http;
using TikTokExplode.Infrastructure.Url;

namespace TikTokExplode.Infrastructure.Repositories;

public sealed class VideoRepository : IVideoRepository
{
    private readonly ITikTokApiClient _apiClient;
    private readonly UrlHandler _urlHandler;
    private readonly IVideoExtractor _extractor;

    public VideoRepository(ITikTokApiClient apiClient, UrlHandler urlHandler, IVideoExtractor extractor)
    {
        _apiClient = apiClient;
        _urlHandler = urlHandler;
        _extractor = extractor;
    }

    public async Task<Video> GetByUrlAsync(string url, CancellationToken ct = default)
    {
        var fullUrl = await _urlHandler.GetFullUrlAsync(url, ct);
        var json = await _apiClient.GetApiResponseAsync(fullUrl, ct);
        var aweme = ResponseParser.ParseFirstAweme(json);

        return _extractor.ExtractVideo(aweme.Video, aweme.AwemeId)
            ?? throw new Domain.Exceptions.ValidationException("Failed to extract video data from response.");
    }
}
