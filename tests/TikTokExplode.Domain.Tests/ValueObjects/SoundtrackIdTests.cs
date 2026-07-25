using TikTokExplode.Domain.ValueObjects;
using FluentAssertions;

namespace TikTokExplode.Domain.Tests.ValueObjects;

public class SoundtrackIdTests
{
    [Fact]
    public void Parse_ValidString_ReturnsSoundtrackId()
    {
        var id = SoundtrackId.Parse("music123");
        id.Value.Should().Be("music123");
    }

    [Fact]
    public void ImplicitConversion_ToString_ReturnsValue()
    {
        SoundtrackId id = "track456";
        string s = id;
        s.Should().Be("track456");
    }
}
