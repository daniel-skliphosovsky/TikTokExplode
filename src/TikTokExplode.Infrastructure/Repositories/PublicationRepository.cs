using TikTokExplode.Domain.Entities;
using TikTokExplode.Domain.Enums;
using TikTokExplode.Domain.Exceptions;
using TikTokExplode.Domain.Interfaces;
using TikTokExplode.Domain.Specifications;
using TikTokExplode.Domain.ValueObjects;
using TikTokExplode.Infrastructure.Extraction;
using TikTokExplode.Infrastructure.Http;
using TikTokExplode.Infrastructure.Url;

namespace TikTokExplode.Infrastructure.Repositories;

public sealed class PublicationRepository : IPublicationRepository
{
    private readonly ITikTokApiClient _apiClient;
    private readonly IVideoExtractor _videoExtractor;
    private readonly IImageExtractor _imageExtractor;
    private readonly IAuthorExtractor _authorExtractor;
    private readonly ISoundtrackExtractor _soundtrackExtractor;
    private readonly IStatsExtractor _statsExtractor;
    private readonly UrlHandler _urlHandler;

    public PublicationRepository(
        ITikTokApiClient apiClient,
        IVideoExtractor videoExtractor,
        IImageExtractor imageExtractor,
        IAuthorExtractor authorExtractor,
        ISoundtrackExtractor soundtrackExtractor,
        IStatsExtractor statsExtractor,
        UrlHandler urlHandler)
    {
        _apiClient = apiClient;
        _videoExtractor = videoExtractor;
        _imageExtractor = imageExtractor;
        _authorExtractor = authorExtractor;
        _soundtrackExtractor = soundtrackExtractor;
        _statsExtractor = statsExtractor;
        _urlHandler = urlHandler;
    }

    public async Task<Publication> GetByUrlAsync(string url, CancellationToken ct = default)
    {
        if (!PublicationUrlValidator.IsValid(url))
            throw new ValidationException("Invalid TikTok URL");

        // Follow redirects to get full URL
        var fullUrl = await _urlHandler.GetFullUrlAsync(url, ct);

        // Get API response
        var jsonResponse = await _apiClient.GetApiResponseAsync(fullUrl, ct);

        // Determine publication type based on URL content before extraction
        var type = fullUrl.Contains("/photo/") ? PublicationType.Images : PublicationType.Video;

        // Extract common entities (always present)
        var author = _authorExtractor.ExtractAuthor(jsonResponse);
        var soundtrack = _soundtrackExtractor.ExtractSoundtrack(jsonResponse);
        var stats = _statsExtractor.ExtractStats(jsonResponse);

        // Extract type-specific entities
        Video? video = null;
        IReadOnlyList<Image>? images = null;

        if (type == PublicationType.Video)
        {
            video = _videoExtractor.ExtractVideo(jsonResponse);
        }
        else if (type == PublicationType.Images)
        {
            images = _imageExtractor.ExtractImages(jsonResponse);
        }

        using var doc = System.Text.Json.JsonDocument.Parse(jsonResponse);
        var awemeList = doc.RootElement.GetProperty("aweme_list");

        var publication = new Publication
        {
            Id = PublicationId.Parse(awemeList[0].GetProperty("aweme_id").GetString() ?? string.Empty),
            Description = awemeList[0].GetProperty("desc").GetString() ?? string.Empty,
            IsAds = awemeList[0].GetProperty("is_ads").GetBoolean(),
            Type = type,
            Author = author,
            Video = video,
            Images = images,
            Soundtrack = soundtrack,
            Stats = stats
        };

        return publication;
    }
}
