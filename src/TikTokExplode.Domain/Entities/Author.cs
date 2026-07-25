using TikTokExplode.Domain.ValueObjects;

namespace TikTokExplode.Domain.Entities;

public sealed class Author
{
    public AuthorId Id { get; init; }
    public string Nickname { get; init; } = string.Empty;
    public bool IsVerified { get; init; }
    public string ThumbAvatarUrl { get; init; } = string.Empty;
    public string MediumAvatarUrl { get; init; } = string.Empty;
    public string Region { get; init; } = string.Empty;
}
