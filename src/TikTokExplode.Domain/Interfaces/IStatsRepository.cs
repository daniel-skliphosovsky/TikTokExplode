using TikTokExplode.Domain.Entities;

namespace TikTokExplode.Domain.Interfaces;

/// <summary>
/// Provides access to TikTok publication statistics.
/// </summary>
public interface IStatsRepository
{
    /// <summary>
    /// Gets statistics for a TikTok publication.
    /// </summary>
    /// <param name="url">TikTok URL.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Stats including play, like, share, and comment counts.</returns>
    Task<Stats> GetByUrlAsync(string url, CancellationToken ct = default);
}
