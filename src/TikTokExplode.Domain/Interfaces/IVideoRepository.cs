using TikTokExplode.Domain.Entities;

namespace TikTokExplode.Domain.Interfaces;

public interface IVideoRepository
{
    Task<Video> GetByUrlAsync(string url, CancellationToken ct = default);
}
