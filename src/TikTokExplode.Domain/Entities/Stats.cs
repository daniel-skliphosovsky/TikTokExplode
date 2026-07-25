namespace TikTokExplode.Domain.Entities;

public sealed class Stats
{
    public long CommentCount { get; init; }
    public long DiggCount { get; init; }
    public long DownloadCount { get; init; }
    public long PlayCount { get; init; }
    public long ShareCount { get; init; }
    public long ForwardCount { get; init; }
    public long RepostCount { get; init; }
}
