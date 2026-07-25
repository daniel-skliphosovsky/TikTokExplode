using TikTokExplode.Domain.Entities;
using TikTokExplode.Domain.ValueObjects;

namespace TikTokExplode.Domain.Interfaces;

/// <summary>
/// Provides access to TikTok publication data.
/// </summary>
public interface IPublicationRepository
{
    /// <summary>
    /// Gets full publication metadata for a TikTok URL.
    /// </summary>
    /// <param name="url">TikTok video or image post URL.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Complete publication with author, media, soundtrack, and stats.</returns>
    Task<Publication> GetByUrlAsync(string url, CancellationToken ct = default);
}
