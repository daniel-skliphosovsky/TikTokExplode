using System.Text.Json;
using TikTokExplode.Domain.Exceptions;
using TikTokExplode.Infrastructure.DTOs;
using TikTokExplode.Infrastructure.Extraction;
using FluentAssertions;

namespace TikTokExplode.Infrastructure.Tests.Extraction;

public class VideoExtractorTests
{
    private readonly VideoExtractor _sut = new();

    [Fact]
    public void ExtractVideo_ValidDto_ReturnsVideo()
    {
        var json = File.ReadAllText("Samples/video_response.json");
        var response = JsonSerializer.Deserialize(json, TikTokApiJsonContext.Default.TikTokApiResponse);
        var aweme = response!.AwemeList![0];
        var dto = aweme.Video;

        var video = _sut.ExtractVideo(dto, aweme.AwemeId);

        video.Should().NotBeNull();
        video!.AwemeId.Should().Be("1234567890");
        video.PlayUrl.Should().Be("https://example.com/video.mp4");
        video.Width.Should().Be(1080);
        video.Height.Should().Be(1920);
        video.Duration.Should().Be(30);
    }

    [Fact]
    public void ExtractVideo_EmptyJson_ThrowsValidationException()
    {
        var json = """{"aweme_list":[]}""";
        Assert.Throws<ValidationException>(() => ResponseParser.ParseFirstAweme(json));
    }

    [Fact]
    public void ExtractVideo_InvalidJson_ThrowsJsonException()
    {
        var json = "not valid json";
        Assert.Throws<JsonException>(() => ResponseParser.ParseFirstAweme(json));
    }

    [Fact]
    public void ExtractVideo_NullDto_ReturnsNull()
    {
        var video = _sut.ExtractVideo(null, "test");

        video.Should().BeNull();
    }
}
