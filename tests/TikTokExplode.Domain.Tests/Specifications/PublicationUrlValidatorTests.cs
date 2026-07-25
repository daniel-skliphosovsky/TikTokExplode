using TikTokExplode.Domain.Specifications;
using FluentAssertions;

namespace TikTokExplode.Domain.Tests.Specifications;

public class PublicationUrlValidatorTests
{
    [Theory]
    [InlineData("https://www.tiktok.com/@user/video/1234567890")]
    [InlineData("https://vm.tiktok.com/ABC123")]
    [InlineData("https://tiktok.com/@user/video/1234567890")]
    [InlineData("http://www.tiktok.com/@user/video/123")]
    [InlineData("https://www.tiktok.com/@user/photo/1234567890")]
    public void IsValid_ValidTikTokUrls_ReturnsTrue(string url)
    {
        PublicationUrlValidator.IsValid(url).Should().BeTrue();
    }

    [Theory]
    [InlineData("https://youtube.com/watch?v=123")]
    [InlineData("https://instagram.com/p/ABC")]
    [InlineData("https://soundcloud.com/user/track")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("not-a-url")]
    [InlineData("https://example.com")]
    public void IsValid_InvalidUrls_ReturnsFalse(string url)
    {
        PublicationUrlValidator.IsValid(url).Should().BeFalse();
    }
}
