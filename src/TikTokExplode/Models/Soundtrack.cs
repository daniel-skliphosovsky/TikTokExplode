namespace TikTokExplode.Publications.Soundtracks;

/// <summary>
/// TikTok publication soundtrack metadata.
/// </summary>
public class Soundtrack
{
    /// <summary>
    /// Soundtrack id.
    /// </summary>
    public ulong Id { get; set; }

    /// <summary>
    /// Soundtrack title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Soundtrack author nickname.
    /// </summary>
    public string Author { get; set; } = string.Empty;

    /// <summary>
    /// Soundtrack audio url.
    /// </summary>
    public string SoundUrl { get; set; } = string.Empty;

    /// <summary>
    /// Large cover image url.
    /// </summary>
    public string LargeCoverUrl { get; set; } = string.Empty;

    /// <summary>
    /// Medium cover image url.
    /// </summary>
    public string MediumCoverUrl { get; set; } = string.Empty;

    /// <summary>
    /// Thumb cover image url.
    /// </summary>
    public string ThumbCoverUrl { get; set; } = string.Empty;
}
