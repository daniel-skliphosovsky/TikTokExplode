using TikTokExplode.Domain.Entities;

namespace TikTokExplode.Infrastructure.Extraction;

public interface IVideoExtractor
{
    Video ExtractVideo(string jsonResponse);
}
