using TikTokExplode.Domain.ValueObjects;
using FluentAssertions;

namespace TikTokExplode.Domain.Tests.ValueObjects;

public class PublicationIdTests
{
    [Fact]
    public void Parse_ValidString_ReturnsPublicationId()
    {
        var id = PublicationId.Parse("1234567890");
        id.Value.Should().Be("1234567890");
    }

    [Fact]
    public void ImplicitConversion_ToString_ReturnsValue()
    {
        PublicationId id = "abc123";
        string s = id;
        s.Should().Be("abc123");
    }

    [Fact]
    public void ImplicitConversion_FromString_ReturnsPublicationId()
    {
        PublicationId id = "test-id";
        id.Value.Should().Be("test-id");
    }

    [Fact]
    public void Equals_SameValue_ReturnsTrue()
    {
        var id1 = PublicationId.Parse("123");
        var id2 = PublicationId.Parse("123");
        id1.Should().Be(id2);
    }

    [Fact]
    public void Equals_DifferentValue_ReturnsFalse()
    {
        var id1 = PublicationId.Parse("123");
        var id2 = PublicationId.Parse("456");
        id1.Should().NotBe(id2);
    }
}
