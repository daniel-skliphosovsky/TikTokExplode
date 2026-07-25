using TikTokExplode.Domain.Entities;

namespace TikTokExplode.Domain.Interfaces;

public interface IAuthorRepository
{
    Task<Author> GetByUrlAsync(string url, CancellationToken ct = default);
}
