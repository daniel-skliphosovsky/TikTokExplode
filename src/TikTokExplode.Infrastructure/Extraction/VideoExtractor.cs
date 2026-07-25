using TikTokExplode.Domain.Entities;
using TikTokExplode.Infrastructure.DTOs;

namespace TikTokExplode.Infrastructure.Extraction;

public class VideoExtractor : IVideoExtractor
{
    public Video? ExtractVideo(VideoDto? dto, string awemeId)
    {
        if (dto is null)
            return null;

        return new Video
        {
            AwemeId = awemeId,
            PlayUrl = dto.PlayAddr?.UrlList?.FirstOrDefault() ?? string.Empty,
            Width = dto.Width,
            Height = dto.Height,
            Duration = dto.Duration
        };
    }
}
