using System.Net.Http;

namespace TikTokExplode.Infrastructure.Tests.Integration;

/// <summary>
/// DelegatingHandler that rewrites outgoing HTTP request URLs
/// to point to a WireMock mock server instead of the real TikTok host.
/// This allows integration tests to intercept and control all HTTP traffic.
/// </summary>
public sealed class MockServerRedirectHandler : DelegatingHandler
{
    private readonly string _mockBaseUrl;

    public MockServerRedirectHandler(string mockBaseUrl)
    {
        _mockBaseUrl = mockBaseUrl.TrimEnd('/');
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.RequestUri is { IsAbsoluteUri: true, Host: not "localhost" } &&
            !request.RequestUri.Host.Contains("127.0.0.1"))
        {
            var pathAndQuery = request.RequestUri.PathAndQuery;
            request.RequestUri = new Uri(_mockBaseUrl + pathAndQuery);
        }

        return base.SendAsync(request, cancellationToken);
    }
}
