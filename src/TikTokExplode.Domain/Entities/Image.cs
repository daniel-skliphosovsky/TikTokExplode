namespace TikTokExplode.Domain.Entities;

public sealed class Image
{
    public string AwemeId { get; init; } = string.Empty;
    public string ImageUrl { get; init; } = string.Empty;
    public int Width { get; init; }
    public int Height { get; init; }
}
