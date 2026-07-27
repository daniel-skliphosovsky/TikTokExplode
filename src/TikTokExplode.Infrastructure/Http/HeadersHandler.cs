using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using TikTokExplode.Infrastructure.Configuration;

namespace TikTokExplode.Infrastructure.Http;

public sealed class HeadersHandler : DelegatingHandler
{
    private readonly TikTokApiOptions _options;

    public HeadersHandler(IOptions<TikTokApiOptions> options)
    {
        _options = options.Value;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var rng = Random.Shared;
        var ua = _options.UserAgents[rng.Next(_options.UserAgents.Length)];

        request.Headers.UserAgent.Clear();
        request.Headers.UserAgent.ParseAdd(ua);
        request.Headers.Accept.ParseAdd("application/json, text/plain, */*");
        request.Headers.AcceptLanguage.ParseAdd("en-US,en;q=0.9");
        request.Headers.Connection.ParseAdd("keep-alive");

        return base.SendAsync(request, cancellationToken);
    }
}
