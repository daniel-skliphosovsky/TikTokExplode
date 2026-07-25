using TikTokExplode.Infrastructure.Extraction;
using FluentAssertions;

namespace TikTokExplode.Infrastructure.Tests.Extraction;

public class StatsExtractorTests
{
    private readonly StatsExtractor _sut = new();

    [Fact]
    public void ExtractStats_ValidJson_ReturnsStats()
    {
        var json = File.ReadAllText("Samples/video_response.json");
        var stats = _sut.ExtractStats(json);

        stats.Should().NotBeNull();
        stats.CommentCount.Should().Be(100);
        stats.DiggCount.Should().Be(500);
        stats.DownloadCount.Should().Be(50);
        stats.PlayCount.Should().Be(10000);
        stats.ShareCount.Should().Be(200);
        stats.ForwardCount.Should().Be(10);
        stats.RepostCount.Should().Be(5);
    }
}
