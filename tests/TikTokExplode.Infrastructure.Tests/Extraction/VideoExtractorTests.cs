using TikTokExplode.Domain.Exceptions;
using TikTokExplode.Infrastructure.Extraction;
using FluentAssertions;

namespace TikTokExplode.Infrastructure.Tests.Extraction;

public class VideoExtractorTests
{
    private readonly VideoExtractor _sut = new();

    [Fact]
    public void ExtractVideo_ValidJson_ReturnsVideo()
    {
        var json = File.ReadAllText("Samples/video_response.json");
        var video = _sut.ExtractVideo(json);

        video.Should().NotBeNull();
        video.AwemeId.Should().Be("1234567890");
        video.PlayUrl.Should().Be("https://example.com/video.mp4");
        video.Width.Should().Be(1080);
        video.Height.Should().Be(1920);
        video.Duration.Should().Be(30);
    }

    [Fact]
    public void ExtractVideo_EmptyJson_ThrowsValidationException()
    {
        var json = """{"aweme_list":[]}""";
        Assert.Throws<ValidationException>(() => _sut.ExtractVideo(json));
    }

    [Fact]
    public void ExtractVideo_InvalidJson_ThrowsApiException()
    {
        var json = "not valid json";
        Assert.Throws<ApiException>(() => _sut.ExtractVideo(json));
    }
}
