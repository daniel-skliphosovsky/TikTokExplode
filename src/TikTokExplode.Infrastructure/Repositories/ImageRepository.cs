using TikTokExplode.Domain.Entities;
using TikTokExplode.Domain.Interfaces;
using TikTokExplode.Infrastructure.Extraction;
using TikTokExplode.Infrastructure.Http;
using TikTokExplode.Infrastructure.Url;

namespace TikTokExplode.Infrastructure.Repositories;

public sealed class ImageRepository : IImageRepository
{
    private readonly ITikTokApiClient _apiClient;
    private readonly IImageExtractor _imageExtractor;
    private readonly UrlHandler _urlHandler;

    public ImageRepository(
        ITikTokApiClient apiClient,
        IImageExtractor imageExtractor,
        UrlHandler urlHandler)
    {
        _apiClient = apiClient;
        _imageExtractor = imageExtractor;
        _urlHandler = urlHandler;
    }

    public async Task<IReadOnlyList<Image>> GetByUrlAsync(string url, CancellationToken ct = default)
    {
        var fullUrl = await _urlHandler.GetFullUrlAsync(url, ct);
        var jsonResponse = await _apiClient.GetApiResponseAsync(fullUrl, ct);
        return _imageExtractor.ExtractImages(jsonResponse);
    }
}
