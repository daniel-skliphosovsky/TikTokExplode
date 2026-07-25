using TikTokExplode.Infrastructure.Extraction;
using FluentAssertions;

namespace TikTokExplode.Infrastructure.Tests.Extraction;

public class ImageExtractorTests
{
    private readonly ImageExtractor _sut = new();

    [Fact]
    public void ExtractImages_ValidJson_ReturnsImages()
    {
        var json = File.ReadAllText("Samples/video_response.json");
        var images = _sut.ExtractImages(json);

        images.Should().NotBeNull();
        images.Should().HaveCount(1);
        images[0].ImageUrl.Should().Be("https://example.com/image1.jpg");
        images[0].Width.Should().Be(1080);
        images[0].Height.Should().Be(1920);
    }
}
