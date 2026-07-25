using TikTokExplode.Domain.Entities;
using TikTokExplode.Infrastructure.DTOs;

namespace TikTokExplode.Infrastructure.Extraction;

public interface IStatsExtractor
{
    Stats ExtractStats(StatisticsDto dto);
}
