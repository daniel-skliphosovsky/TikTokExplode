using System.Text.Json;
using TikTokExplode.Domain.Exceptions;
using TikTokExplode.Infrastructure.DTOs;
using TikTokExplode.Infrastructure.Extraction;
using FluentAssertions;

namespace TikTokExplode.Infrastructure.Tests.Extraction;

public class AuthorExtractorTests
{
    private readonly AuthorExtractor _sut = new();

    [Fact]
    public void ExtractAuthor_ValidDto_ReturnsAuthor()
    {
        var json = File.ReadAllText("Samples/video_response.json");
        var response = JsonSerializer.Deserialize(json, TikTokApiJsonContext.Default.TikTokApiResponse);
        var dto = response!.AwemeList![0].Author!;

        var author = _sut.ExtractAuthor(dto);

        author.Should().NotBeNull();
        author.Id.Value.Should().Be("user123");
        author.Nickname.Should().Be("TestUser");
        author.IsVerified.Should().BeTrue();
        author.Region.Should().Be("US");
        author.ThumbAvatarUrl.Should().NotBeNullOrEmpty();
        author.MediumAvatarUrl.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void ExtractAuthor_EmptyJson_ThrowsValidationException()
    {
        var json = """{"aweme_list":[]}""";
        Assert.Throws<ValidationException>(() => ResponseParser.ParseFirstAweme(json));
    }

    [Fact]
    public void ExtractAuthor_InvalidJson_ThrowsJsonException()
    {
        var json = "not valid json";
        Assert.Throws<JsonException>(() => ResponseParser.ParseFirstAweme(json));
    }
}
