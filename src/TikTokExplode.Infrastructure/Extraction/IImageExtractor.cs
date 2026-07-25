using TikTokExplode.Domain.Entities;

namespace TikTokExplode.Infrastructure.Extraction;

public interface IImageExtractor
{
    IReadOnlyList<Image> ExtractImages(string jsonResponse);
}
