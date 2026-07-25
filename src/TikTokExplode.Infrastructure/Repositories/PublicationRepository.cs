using TikTokExplode.Domain.Entities;
using TikTokExplode.Domain.Enums;
using TikTokExplode.Domain.Exceptions;
using TikTokExplode.Domain.Interfaces;
using TikTokExplode.Domain.Specifications;
using TikTokExplode.Infrastructure.Extraction;
using TikTokExplode.Infrastructure.Http;
using TikTokExplode.Infrastructure.Url;

namespace TikTokExplode.Infrastructure.Repositories;

public sealed class PublicationRepository : IPublicationRepository
{
    private readonly ITikTokApiClient _apiClient;
    private readonly UrlHandler _urlHandler;
    private readonly IAuthorExtractor _authorExtractor;
    private readonly IVideoExtractor _videoExtractor;
    private readonly IImageExtractor _imageExtractor;
    private readonly ISoundtrackExtractor _soundtrackExtractor;
    private readonly IStatsExtractor _statsExtractor;

    public PublicationRepository(
        ITikTokApiClient apiClient,
        UrlHandler urlHandler,
        IAuthorExtractor authorExtractor,
        IVideoExtractor videoExtractor,
        IImageExtractor imageExtractor,
        ISoundtrackExtractor soundtrackExtractor,
        IStatsExtractor statsExtractor)
    {
        _apiClient = apiClient;
        _urlHandler = urlHandler;
        _authorExtractor = authorExtractor;
        _videoExtractor = videoExtractor;
        _imageExtractor = imageExtractor;
        _soundtrackExtractor = soundtrackExtractor;
        _statsExtractor = statsExtractor;
    }

    public async Task<Publication> GetByUrlAsync(string url, CancellationToken ct = default)
    {
        if (!PublicationUrlValidator.IsValid(url))
            throw new ValidationException("Invalid TikTok URL format.");

        var fullUrl = await _urlHandler.GetFullUrlAsync(url, ct);
        var json = await _apiClient.GetApiResponseAsync(fullUrl, ct);
        var aweme = ResponseParser.ParseFirstAweme(json);

        var type = fullUrl.Contains("/photo/", StringComparison.OrdinalIgnoreCase)
            ? PublicationType.Images
            : PublicationType.Video;

        var author = _authorExtractor.ExtractAuthor(aweme.Author
            ?? throw new ValidationException("Failed to extract author data."));

        var soundtrack = _soundtrackExtractor.ExtractSoundtrack(aweme.Music);
        var stats = _statsExtractor.ExtractStats(aweme.Statistics
            ?? throw new ValidationException("Failed to extract statistics data."));

        Video? video = null;
        IReadOnlyList<Image>? images = null;

        if (type == PublicationType.Video)
        {
            video = _videoExtractor.ExtractVideo(aweme.Video, aweme.AwemeId);
        }
        else
        {
            var extractedImages = _imageExtractor.ExtractImages(aweme.ImagePostInfo);
            images = extractedImages.Select(img => new Image
            {
                AwemeId = aweme.AwemeId,
                ImageUrl = img.ImageUrl,
                Width = img.Width,
                Height = img.Height
            }).ToList().AsReadOnly();
        }

        return new Publication
        {
            Id = new(url),
            Description = aweme.Description,
            IsAds = aweme.IsAds,
            Type = type,
            Author = author,
            Video = video,
            Images = images,
            Soundtrack = soundtrack ?? new Soundtrack { Id = new(string.Empty) },
            Stats = stats
        };
    }
}
