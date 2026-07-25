using TikTokExplode.Domain.ValueObjects;
using FluentAssertions;

namespace TikTokExplode.Domain.Tests.ValueObjects;

public class AuthorIdTests
{
    [Fact]
    public void Parse_ValidString_ReturnsAuthorId()
    {
        var id = AuthorId.Parse("user123");
        id.Value.Should().Be("user123");
    }

    [Fact]
    public void ImplicitConversion_ToString_ReturnsValue()
    {
        AuthorId id = "user456";
        string s = id;
        s.Should().Be("user456");
    }

    [Fact]
    public void Equals_SameValue_ReturnsTrue()
    {
        var id1 = AuthorId.Parse("u1");
        var id2 = AuthorId.Parse("u1");
        id1.Should().Be(id2);
    }
}
