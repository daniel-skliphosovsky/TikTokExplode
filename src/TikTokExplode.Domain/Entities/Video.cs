namespace TikTokExplode.Domain.Entities;

public sealed class Video
{
    public string AwemeId { get; init; } = string.Empty;
    public string PlayUrl { get; init; } = string.Empty;
    public int Width { get; init; }
    public int Height { get; init; }
    public int Duration { get; init; }
}
