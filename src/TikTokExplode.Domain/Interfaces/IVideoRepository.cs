using TikTokExplode.Domain.Entities;

namespace TikTokExplode.Domain.Interfaces;

/// <summary>
/// Provides access to TikTok video metadata.
/// </summary>
public interface IVideoRepository
{
    /// <summary>
    /// Gets video metadata for a TikTok URL.
    /// </summary>
    /// <param name="url">TikTok video URL.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Video metadata including play URL and dimensions.</returns>
    Task<Video> GetByUrlAsync(string url, CancellationToken ct = default);
}
