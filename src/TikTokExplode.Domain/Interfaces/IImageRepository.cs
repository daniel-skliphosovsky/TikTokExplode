using TikTokExplode.Domain.Entities;

namespace TikTokExplode.Domain.Interfaces;

/// <summary>
/// Provides access to TikTok image post metadata.
/// </summary>
public interface IImageRepository
{
    /// <summary>
    /// Gets image metadata for a TikTok photo post URL.
    /// </summary>
    /// <param name="url">TikTok photo post URL.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>List of images with URLs and dimensions.</returns>
    Task<IReadOnlyList<Image>> GetByUrlAsync(string url, CancellationToken ct = default);
}
