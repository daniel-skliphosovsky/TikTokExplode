using TikTokExplode.Domain.Entities;

namespace TikTokExplode.Domain.Interfaces;

/// <summary>
/// Provides access to TikTok author/user metadata.
/// </summary>
public interface IAuthorRepository
{
    /// <summary>
    /// Gets author metadata for a TikTok URL.
    /// </summary>
    /// <param name="url">TikTok URL.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Author metadata including nickname and avatar URLs.</returns>
    Task<Author> GetByUrlAsync(string url, CancellationToken ct = default);
}
