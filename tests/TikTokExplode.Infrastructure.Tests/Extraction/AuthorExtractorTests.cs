using TikTokExplode.Domain.Exceptions;
using TikTokExplode.Infrastructure.Extraction;
using FluentAssertions;

namespace TikTokExplode.Infrastructure.Tests.Extraction;

public class AuthorExtractorTests
{
    private readonly AuthorExtractor _sut = new();

    [Fact]
    public void ExtractAuthor_ValidJson_ReturnsAuthor()
    {
        var json = File.ReadAllText("Samples/video_response.json");
        var author = _sut.ExtractAuthor(json);

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
        Assert.Throws<ValidationException>(() => _sut.ExtractAuthor(json));
    }
}
