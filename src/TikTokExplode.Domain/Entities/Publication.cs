using TikTokExplode.Domain.Enums;
using TikTokExplode.Domain.ValueObjects;

namespace TikTokExplode.Domain.Entities;

public sealed class Publication
{
    public PublicationId Id { get; init; }
    public string Description { get; init; } = string.Empty;
    public bool IsAds { get; init; }
    public PublicationType Type { get; init; }
    public Author Author { get; init; } = null!;
    public Video? Video { get; init; }
    public IReadOnlyList<Image>? Images { get; init; }
    public Soundtrack Soundtrack { get; init; } = null!;
    public Stats Stats { get; init; } = null!;
}
