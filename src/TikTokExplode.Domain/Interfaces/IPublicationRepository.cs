using TikTokExplode.Domain.Entities;
using TikTokExplode.Domain.ValueObjects;

namespace TikTokExplode.Domain.Interfaces;

public interface IPublicationRepository
{
    Task<Publication> GetByUrlAsync(string url, CancellationToken ct = default);
}
