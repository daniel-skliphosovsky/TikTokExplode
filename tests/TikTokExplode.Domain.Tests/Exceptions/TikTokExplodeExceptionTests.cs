using TikTokExplode.Domain.Exceptions;
using FluentAssertions;

namespace TikTokExplode.Domain.Tests.Exceptions;

public class TikTokExplodeExceptionTests
{
    [Fact]
    public void TikTokExplodeException_ShouldBeAbstract()
    {
        typeof(TikTokExplodeException).IsAbstract.Should().BeTrue();
    }

    [Fact]
    public void TikTokExplodeException_ShouldInheritFromException()
    {
        typeof(TikTokExplodeException).Should().BeDerivedFrom<Exception>();
    }

    [Fact]
    public void ApiException_ShouldStoreStatusCodeAndBody()
    {
        var ex = new ApiException("API error", 403, "forbidden");
        ex.StatusCode.Should().Be(403);
        ex.ResponseBody.Should().Be("forbidden");
        ex.Message.Should().Be("API error");
    }

    [Fact]
    public void ApiException_ShouldInheritFromTikTokExplodeException()
    {
        var ex = new ApiException("test", 500, "error");
        ex.Should().BeAssignableTo<TikTokExplodeException>();
    }

    [Fact]
    public void ValidationException_ShouldStoreMessage()
    {
        var ex = new ValidationException("Invalid input");
        ex.Message.Should().Be("Invalid input");
    }

    [Fact]
    public void ValidationException_ShouldInheritFromTikTokExplodeException()
    {
        var ex = new ValidationException("test");
        ex.Should().BeAssignableTo<TikTokExplodeException>();
    }
}
