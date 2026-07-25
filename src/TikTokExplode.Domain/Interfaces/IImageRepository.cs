using TikTokExplode.Domain.Entities;

namespace TikTokExplode.Domain.Interfaces;

public interface IImageRepository
{
    Task<IReadOnlyList<Image>> GetByUrlAsync(string url, CancellationToken ct = default);
}
