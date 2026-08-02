using System.Text.Json;
using TikTokExplode.Exceptions;
using TikTokExplode.Publications;
using TikTokExplode.Publications.Authors;
using TikTokExplode.Publications.Images;
using TikTokExplode.Publications.Soundtracks;
using TikTokExplode.Publications.Statistics;
using TikTokExplode.Publications.Videos;

namespace TikTokExplode;

/// <summary>
/// Parses TikTok API responses and maps them to public models.
/// </summary>
internal static class ApiResponseParser
{
    public static AwemeDto ParseFirstAweme(string json)
    {
        TikTokApiResponse? response;
        try
        {
            response = JsonSerializer.Deserialize(json, TikTokApiJsonContext.Default.TikTokApiResponse);
        }
        catch (JsonException ex)
        {
            throw new ValidationException($"Error parsing TikTok API response: {ex.Message}", ex);
        }

        return response?.AwemeList?.FirstOrDefault()
            ?? throw new ValidationException("TikTok API returned an empty response. The post may not exist or the URL may be invalid.");
    }

    public static Publication ParsePublication(AwemeDto aweme, PublicationClient.PublicationType type)
    {
        return new Publication
        {
            Id = aweme.AwemeId,
            Description = aweme.Description,
            IsAds = aweme.IsAds,
            Author = ParseAuthor(aweme.Author),
            Statistics = ParseStats(aweme.Statistics),
            Soundtrack = ParseSoundtrack(aweme.Music),
            Video = type == PublicationClient.PublicationType.Video ? ParseVideo(aweme.Video, aweme.AwemeId) : null,
            Images = type == PublicationClient.PublicationType.Images ? ParseImages(aweme.ImagePostInfo, aweme.AwemeId) : null
        };
    }

    public static Author ParseAuthor(AuthorDto? dto)
    {
        if (dto is null)
            throw new ValidationException("Failed to extract author data from response.");

        // TikTok exposes verification both as a "verified" flag and a "custom_verify" string.
        bool isVerified = dto.Verified || (!string.IsNullOrEmpty(dto.CustomVerify) && dto.CustomVerify != "0");

        return new Author
        {
            UserId = dto.Uid,
            Nickname = dto.Nickname,
            IsVerified = isVerified,
            ThumbAvatarUrl = FirstUrl(dto.AvatarThumb),
            MediumAvatarUrl = FirstUrl(dto.AvatarMedium),
            Region = dto.Region
        };
    }

    public static Video? ParseVideo(VideoDto? dto, string awemeId)
    {
        if (dto is null)
            return null;

        return new Video
        {
            AwemeId = awemeId,
            Url = FirstUrl(dto.PlayAddr),
            Width = dto.Width,
            Height = dto.Height,
            Duration = (ulong)Math.Max(0, dto.Duration)
        };
    }

    public static List<Image>? ParseImages(ImagePostInfoDto? dto, string awemeId)
    {
        if (dto?.Images is not { Count: > 0 } images)
            return null;

        return images
            .Select(image => new Image
            {
                AwemeId = awemeId,
                Url = image.DisplayImage?.UrlList?.FirstOrDefault() ?? string.Empty,
                Width = image.DisplayImage?.Width ?? 0,
                Height = image.DisplayImage?.Height ?? 0
            })
            .ToList();
    }

    public static Soundtrack ParseSoundtrack(MusicDto? dto)
    {
        if (dto is null)
            throw new ValidationException("Failed to extract soundtrack data from response.");

        ulong.TryParse(dto.Id.ToString(), out ulong id);

        return new Soundtrack
        {
            Id = id,
            Title = dto.Title,
            Author = dto.Author,
            SoundUrl = FirstUrl(dto.PlayUrl),
            LargeCoverUrl = FirstUrl(dto.CoverLarge),
            MediumCoverUrl = FirstUrl(dto.CoverMedium),
            ThumbCoverUrl = FirstUrl(dto.CoverThumb)
        };
    }

    public static Stats ParseStats(StatisticsDto? dto)
    {
        if (dto is null)
            throw new ValidationException("Failed to extract statistics data from response.");

        return new Stats
        {
            CommentCount = (ulong)Math.Max(0, dto.CommentCount),
            DiggCount = (ulong)Math.Max(0, dto.DiggCount),
            DownloadCount = (ulong)Math.Max(0, dto.DownloadCount),
            PlayCount = (ulong)Math.Max(0, dto.PlayCount),
            ShareCount = (ulong)Math.Max(0, dto.ShareCount),
            ForwardCount = (ulong)Math.Max(0, dto.ForwardCount),
            RepostCount = (ulong)Math.Max(0, dto.RepostCount)
        };
    }

    private static string FirstUrl(UrlListDto? dto)
        => dto?.UrlList?.FirstOrDefault() ?? string.Empty;
}
