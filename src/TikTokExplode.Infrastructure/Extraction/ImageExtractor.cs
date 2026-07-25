using TikTokExplode.Domain.Entities;
using TikTokExplode.Infrastructure.DTOs;

namespace TikTokExplode.Infrastructure.Extraction;

public class ImageExtractor : IImageExtractor
{
    public IReadOnlyList<Image> ExtractImages(ImagePostInfoDto? dto)
    {
        if (dto?.Images is not { Count: > 0 } images)
            return Array.Empty<Image>();

        return images.Select(img => new Image
        {
            AwemeId = string.Empty,
            ImageUrl = img.DisplayImage?.UrlList?.FirstOrDefault() ?? string.Empty,
            Width = img.DisplayImage?.Width ?? 0,
            Height = img.DisplayImage?.Height ?? 0
        }).ToList().AsReadOnly();
    }
}
