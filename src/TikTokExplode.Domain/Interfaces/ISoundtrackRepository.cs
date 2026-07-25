using TikTokExplode.Domain.Entities;

namespace TikTokExplode.Domain.Interfaces;

/// <summary>
/// Provides access to TikTok soundtrack/music metadata.
/// </summary>
public interface ISoundtrackRepository
{
    /// <summary>
    /// Gets soundtrack metadata for a TikTok URL.
    /// </summary>
    /// <param name="url">TikTok URL.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Soundtrack metadata including title and audio URL.</returns>
    Task<Soundtrack> GetByUrlAsync(string url, CancellationToken ct = default);
}
