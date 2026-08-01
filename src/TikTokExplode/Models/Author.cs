namespace TikTokExplode.Publications.Authors;

/// <summary>
/// TikTok publication author metadata.
/// </summary>
public class Author
{
    /// <summary>
    /// Author (user) id.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Author nickname.
    /// </summary>
    public string Nickname { get; set; } = string.Empty;

    /// <summary>
    /// Whether the author account is verified.
    /// </summary>
    public bool IsVerified { get; set; }

    /// <summary>
    /// Small avatar image url.
    /// </summary>
    public string ThumbAvatarUrl { get; set; } = string.Empty;

    /// <summary>
    /// Medium avatar image url.
    /// </summary>
    public string MediumAvatarUrl { get; set; } = string.Empty;

    /// <summary>
    /// Author region code.
    /// </summary>
    public string Region { get; set; } = string.Empty;
}
