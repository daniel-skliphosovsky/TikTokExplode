using System.Net.Http.Headers;
using TikTokExplode.Infrastructure.Http;
using FluentAssertions;

namespace TikTokExplode.Infrastructure.Tests.Http;

public class HeadersProviderTests
{
    [Fact]
    public void ApplyHeaders_SetsUserAgent()
    {
        var userAgents = new[] { "TestAgent/1.0" };
        var provider = new HeadersProvider(userAgents);
        var request = new HttpRequestMessage();

        provider.ApplyHeaders(request.Headers);

        request.Headers.UserAgent.ToString().Should().Be("TestAgent/1.0");
    }

    [Fact]
    public void ApplyHeaders_SetsAccept()
    {
        var provider = new HeadersProvider(new[] { "TestAgent/1.0" });
        var request = new HttpRequestMessage();

        provider.ApplyHeaders(request.Headers);

        request.Headers.Accept.ToString().Should().Contain("application/json");
    }
}
