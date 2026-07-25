using System.Text.Json;
using TikTokExplode.Domain.Exceptions;
using TikTokExplode.Infrastructure.DTOs;
using TikTokExplode.Infrastructure.Extraction;
using FluentAssertions;

namespace TikTokExplode.Infrastructure.Tests.Extraction;

public class ImageExtractorTests
{
    private readonly ImageExtractor _sut = new();

    [Fact]
    public void ExtractImages_ValidDto_ReturnsImages()
    {
        var json = File.ReadAllText("Samples/video_response.json");
        var response = JsonSerializer.Deserialize(json, TikTokApiJsonContext.Default.TikTokApiResponse);
        var dto = response!.AwemeList![0].ImagePostInfo;

        var images = _sut.ExtractImages(dto);

        images.Should().NotBeNull();
        images.Should().HaveCount(1);
        images[0].ImageUrl.Should().Be("https://example.com/image1.jpg");
        images[0].Width.Should().Be(1080);
        images[0].Height.Should().Be(1920);
    }

    [Fact]
    public void ExtractImages_EmptyJson_ThrowsValidationException()
    {
        var json = """{"aweme_list":[]}""";
        Assert.Throws<ValidationException>(() => ResponseParser.ParseFirstAweme(json));
    }

    [Fact]
    public void ExtractImages_InvalidJson_ThrowsJsonException()
    {
        var json = "not valid json";
        Assert.Throws<JsonException>(() => ResponseParser.ParseFirstAweme(json));
    }

    [Fact]
    public void ExtractImages_NullDto_ReturnsEmptyList()
    {
        var images = _sut.ExtractImages(null);

        images.Should().NotBeNull();
        images.Should().BeEmpty();
    }
}
