using TikTokExplode.Domain.Enums;
using FluentAssertions;

namespace TikTokExplode.Domain.Tests.Enums;

public class PublicationTypeTests
{
    [Fact]
    public void PublicationType_ShouldHaveThreeMembers()
    {
        Enum.GetValues<PublicationType>().Should().HaveCount(3);
    }

    [Fact]
    public void PublicationType_Unknown_ShouldBeDefault()
    {
        default(PublicationType).Should().Be(PublicationType.Unknown);
    }

    [Fact]
    public void PublicationType_Values_ShouldBeCorrect()
    {
        ((int)PublicationType.Unknown).Should().Be(0);
        ((int)PublicationType.Video).Should().Be(1);
        ((int)PublicationType.Images).Should().Be(2);
    }
}
