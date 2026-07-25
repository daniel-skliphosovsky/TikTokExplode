using TikTokExplode.Domain.Entities;
using TikTokExplode.Domain.Interfaces;
using TikTokExplode.Infrastructure.Extraction;
using TikTokExplode.Infrastructure.Http;
using TikTokExplode.Infrastructure.Url;

namespace TikTokExplode.Infrastructure.Repositories;

public sealed class ImageRepository : IImageRepository
{
    private readonly ITikTokApiClient _apiClient;
    private readonly UrlHandler _urlHandler;
    private readonly IImageExtractor _extractor;

    public ImageRepository(ITikTokApiClient apiClient, UrlHandler urlHandler, IImageExtractor extractor)
    {
        _apiClient = apiClient;
        _urlHandler = urlHandler;
        _extractor = extractor;
    }

    public async Task<IReadOnlyList<Image>> GetByUrlAsync(string url, CancellationToken ct = default)
    {
        var fullUrl = await _urlHandler.GetFullUrlAsync(url, ct);
        var json = await _apiClient.GetApiResponseAsync(fullUrl, ct);
        var aweme = ResponseParser.ParseFirstAweme(json);

        return _extractor.ExtractImages(aweme.ImagePostInfo);
    }
}
