using TikTokExplode.Domain.Entities;

namespace TikTokExplode.Infrastructure.Extraction;

public interface IStatsExtractor
{
    Stats ExtractStats(string jsonResponse);
}
