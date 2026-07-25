using TikTokExplode.Domain.ValueObjects;

namespace TikTokExplode.Domain.Entities;

public sealed class Soundtrack
{
    public SoundtrackId Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string AuthorName { get; init; } = string.Empty;
    public string SoundUrl { get; init; } = string.Empty;
    public string LargeCoverUrl { get; init; } = string.Empty;
    public string MediumCoverUrl { get; init; } = string.Empty;
    public string ThumbCoverUrl { get; init; } = string.Empty;
}
