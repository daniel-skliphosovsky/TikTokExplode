using TikTokExplode.Domain.Entities;
using TikTokExplode.Infrastructure.DTOs;

namespace TikTokExplode.Infrastructure.Extraction;

public class StatsExtractor : IStatsExtractor
{
    public Stats ExtractStats(StatisticsDto dto)
    {
        return new Stats
        {
            CommentCount = dto.CommentCount,
            DiggCount = dto.DiggCount,
            DownloadCount = dto.DownloadCount,
            PlayCount = dto.PlayCount,
            ShareCount = dto.ShareCount,
            ForwardCount = dto.ForwardCount,
            RepostCount = dto.RepostCount
        };
    }
}
