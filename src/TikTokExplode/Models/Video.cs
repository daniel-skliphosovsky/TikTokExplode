namespace TikTokExplode.Publications.Videos;

/// <summary>
/// TikTok publication video metadata.
/// </summary>
public class Video
{
    /// <summary>
    /// Publication (aweme) id.
    /// </summary>
    public string AwemeId { get; set; } = string.Empty;

    /// <summary>
    /// Direct video download url.
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Video width in pixels.
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// Video height in pixels.
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// Video duration in milliseconds.
    /// </summary>
    public ulong Duration { get; set; }
}
