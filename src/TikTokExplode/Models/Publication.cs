using TikTokExplode.Publications.Authors;
using TikTokExplode.Publications.Images;
using TikTokExplode.Publications.Soundtracks;
using TikTokExplode.Publications.Statistics;
using TikTokExplode.Publications.Videos;

namespace TikTokExplode.Publications;

/// <summary>
/// TikTok publication metadata.
/// </summary>
public class Publication
{
    /// <summary>
    /// Publication (aweme) id.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Publication description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Whether the publication is an advertisement.
    /// </summary>
    public bool IsAds { get; set; }

    /// <summary>
    /// Publication author.
    /// </summary>
    public Author Author { get; set; } = null!;

    /// <summary>
    /// Publication statistics. Present for both videos and image posts.
    /// </summary>
    public Stats Statistics { get; set; } = null!;

    /// <summary>
    /// Publication soundtrack.
    /// </summary>
    public Soundtrack Soundtrack { get; set; } = null!;

    /// <summary>
    /// Publication video. Present only for video posts.
    /// </summary>
    public Video? Video { get; set; }

    /// <summary>
    /// Publication images. Present only for image posts.
    /// </summary>
    public List<Image>? Images { get; set; }
}
