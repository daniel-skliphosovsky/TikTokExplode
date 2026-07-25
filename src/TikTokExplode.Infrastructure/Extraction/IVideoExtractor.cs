using TikTokExplode.Domain.Entities;
using TikTokExplode.Infrastructure.DTOs;

namespace TikTokExplode.Infrastructure.Extraction;

public interface IVideoExtractor
{
    Video? ExtractVideo(VideoDto? dto, string awemeId);
}
