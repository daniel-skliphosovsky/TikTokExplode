using TikTokExplode.Domain.Entities;

namespace TikTokExplode.Domain.Interfaces;

public interface ISoundtrackRepository
{
    Task<Soundtrack> GetByUrlAsync(string url, CancellationToken ct = default);
}
