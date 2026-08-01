namespace TikTokExplode.Publications.Images;

/// <summary>
/// TikTok publication image metadata.
/// </summary>
public class Image
{
    /// <summary>
    /// Publication (aweme) id.
    /// </summary>
    public string AwemeId { get; set; } = string.Empty;

    /// <summary>
    /// Direct image download url.
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Image width in pixels.
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// Image height in pixels.
    /// </summary>
    public int Height { get; set; }
}
