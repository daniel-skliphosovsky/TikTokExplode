using TikTokExplode.Domain.Entities;
using TikTokExplode.Infrastructure.DTOs;

namespace TikTokExplode.Infrastructure.Extraction;

public interface IImageExtractor
{
    IReadOnlyList<Image> ExtractImages(ImagePostInfoDto? dto);
}
