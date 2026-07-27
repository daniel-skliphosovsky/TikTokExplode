using TikTokExplode.Domain.Specifications;
using FluentAssertions;

namespace TikTokExplode.Domain.Tests.Specifications;

public class PublicationUrlSpecificationTests
{
    private static readonly IPublicationUrlSpecification Sut = new PublicationUrlSpecification();

    [Theory]
    [InlineData("https://www.tiktok.com/@user/video/1234567890")]
    [InlineData("https://vm.tiktok.com/ABC123")]
    [InlineData("https://tiktok.com/@user/video/1234567890")]
    [InlineData("http://www.tiktok.com/@user/video/123")]
    [InlineData("https://www.tiktok.com/@user/photo/1234567890")]
    public void IsSatisfiedBy_ValidTikTokUrls_ReturnsTrue(string? url)
    {
        Sut.IsSatisfiedBy(url).Should().BeTrue();
    }

    [Theory]
    [InlineData("https://youtube.com/watch?v=123")]
    [InlineData("https://instagram.com/p/ABC")]
    [InlineData("https://soundcloud.com/user/track")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("not-a-url")]
    [InlineData("https://example.com")]
    public void IsSatisfiedBy_InvalidUrls_ReturnsFalse(string? url)
    {
        Sut.IsSatisfiedBy(url).Should().BeFalse();
    }

    [Theory]
    [InlineData("https://www.tiktok.com/@user/video/1234567890", "Invalid TikTok URL. URL must be from tiktok.com or vm.tiktok.com.")]
    [InlineData("", "URL cannot be null or empty.")]
    [InlineData(null, "URL cannot be null or empty.")]
    [InlineData("not-a-url", "Invalid TikTok URL. URL must be from tiktok.com or vm.tiktok.com.")]
    public void GetErrorMessage_ReturnsExpectedMessage(string? url, string expectedMessage)
    {
        Sut.GetErrorMessage(url).Should().Be(expectedMessage);
    }
}
