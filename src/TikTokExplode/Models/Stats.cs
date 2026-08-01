namespace TikTokExplode.Publications.Statistics;

/// <summary>
/// TikTok publication statistics.
/// </summary>
public class Stats
{
    /// <summary>
    /// Number of comments.
    /// </summary>
    public ulong CommentCount { get; set; }

    /// <summary>
    /// Number of likes.
    /// </summary>
    public ulong DiggCount { get; set; }

    /// <summary>
    /// Number of downloads.
    /// </summary>
    public ulong DownloadCount { get; set; }

    /// <summary>
    /// Number of plays.
    /// </summary>
    public ulong PlayCount { get; set; }

    /// <summary>
    /// Number of shares.
    /// </summary>
    public ulong ShareCount { get; set; }

    /// <summary>
    /// Number of forwards.
    /// </summary>
    public ulong ForwardCount { get; set; }

    /// <summary>
    /// Number of reposts.
    /// </summary>
    public ulong RepostCount { get; set; }
}
