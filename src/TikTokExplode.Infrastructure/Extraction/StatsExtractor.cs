using System.Text.Json;
using TikTokExplode.Domain.Entities;
using TikTokExplode.Domain.Exceptions;

namespace TikTokExplode.Infrastructure.Extraction;

public sealed class StatsExtractor : IStatsExtractor
{
    public Stats ExtractStats(string jsonResponse)
    {
        try
        {
            using var doc = JsonDocument.Parse(jsonResponse);
            var awemeList = doc.RootElement.GetProperty("aweme_list");
            var statsData = awemeList[0].GetProperty("statistics");

            var stats = new Stats
            {
                CommentCount = statsData.GetProperty("comment_count").GetInt64(),
                DiggCount = statsData.GetProperty("digg_count").GetInt64(),
                DownloadCount = statsData.GetProperty("download_count").GetInt64(),
                PlayCount = statsData.GetProperty("play_count").GetInt64(),
                ShareCount = statsData.GetProperty("share_count").GetInt64(),
                ForwardCount = statsData.GetProperty("forward_count").GetInt64(),
                RepostCount = statsData.TryGetProperty("repost_count", out var repost) ? repost.GetInt64() : 0
            };

            return stats;
        }
        catch (JsonException ex)
        {
            throw new ApiException("Failed to parse stats JSON", 0, jsonResponse, ex);
        }
    }
}
