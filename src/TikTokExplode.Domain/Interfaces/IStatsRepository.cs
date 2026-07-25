using TikTokExplode.Domain.Entities;

namespace TikTokExplode.Domain.Interfaces;

public interface IStatsRepository
{
    Task<Stats> GetByUrlAsync(string url, CancellationToken ct = default);
}
