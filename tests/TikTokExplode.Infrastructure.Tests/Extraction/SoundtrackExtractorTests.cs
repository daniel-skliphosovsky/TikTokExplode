using System.Text.Json;
using TikTokExplode.Domain.Exceptions;
using TikTokExplode.Infrastructure.DTOs;
using TikTokExplode.Infrastructure.Extraction;
using FluentAssertions;

namespace TikTokExplode.Infrastructure.Tests.Extraction;

public class SoundtrackExtractorTests
{
    private readonly SoundtrackExtractor _sut = new();

    [Fact]
    public void ExtractSoundtrack_ValidDto_ReturnsSoundtrack()
    {
        var json = File.ReadAllText("Samples/video_response.json");
        var response = JsonSerializer.Deserialize(json, TikTokApiJsonContext.Default.TikTokApiResponse);
        var dto = response!.AwemeList![0].Music;

        var soundtrack = _sut.ExtractSoundtrack(dto);

        soundtrack.Should().NotBeNull();
        soundtrack!.Id.Value.Should().Be("music123");
        soundtrack.Title.Should().Be("Test Song");
        soundtrack.AuthorName.Should().Be("Test Artist");
        soundtrack.SoundUrl.Should().Be("https://example.com/sound.mp3");
    }

    [Fact]
    public void ExtractSoundtrack_EmptyJson_ThrowsValidationException()
    {
        var json = """{"aweme_list":[]}""";
        Assert.Throws<ValidationException>(() => ResponseParser.ParseFirstAweme(json));
    }

    [Fact]
    public void ExtractSoundtrack_InvalidJson_ThrowsJsonException()
    {
        var json = "not valid json";
        Assert.Throws<JsonException>(() => ResponseParser.ParseFirstAweme(json));
    }

    [Fact]
    public void ExtractSoundtrack_NullDto_ReturnsNull()
    {
        var soundtrack = _sut.ExtractSoundtrack(null);

        soundtrack.Should().BeNull();
    }
}
