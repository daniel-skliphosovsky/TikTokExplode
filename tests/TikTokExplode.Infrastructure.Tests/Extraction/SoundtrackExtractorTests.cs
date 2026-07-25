using TikTokExplode.Domain.Exceptions;
using TikTokExplode.Infrastructure.Extraction;
using FluentAssertions;

namespace TikTokExplode.Infrastructure.Tests.Extraction;

public class SoundtrackExtractorTests
{
    private readonly SoundtrackExtractor _sut = new();

    [Fact]
    public void ExtractSoundtrack_ValidJson_ReturnsSoundtrack()
    {
        var json = File.ReadAllText("Samples/video_response.json");
        var soundtrack = _sut.ExtractSoundtrack(json);

        soundtrack.Should().NotBeNull();
        soundtrack.Id.Value.Should().Be("music123");
        soundtrack.Title.Should().Be("Test Song");
        soundtrack.AuthorName.Should().Be("Test Artist");
        soundtrack.SoundUrl.Should().Be("https://example.com/sound.mp3");
    }

    [Fact]
    public void ExtractSoundtrack_EmptyJson_ThrowsValidationException()
    {
        var json = """{"aweme_list":[]}""";
        Assert.Throws<ValidationException>(() => _sut.ExtractSoundtrack(json));
    }

    [Fact]
    public void ExtractSoundtrack_InvalidJson_ThrowsApiException()
    {
        var json = "not valid json";
        Assert.Throws<ApiException>(() => _sut.ExtractSoundtrack(json));
    }
}
