using System.Net.Http.Headers;

namespace TikTokExplode.Infrastructure.Http;

public sealed class HeadersProvider
{
    private readonly string[] _userAgents;

    public HeadersProvider(string[] userAgents)
    {
        _userAgents = userAgents;
    }

    public void ApplyHeaders(HttpRequestHeaders headers)
    {
        var rng = Random.Shared;
        var ua = _userAgents[rng.Next(_userAgents.Length)];
        headers.UserAgent.ParseAdd(ua);
        headers.Accept.ParseAdd("application/json, text/plain, */*");
        headers.AcceptLanguage.ParseAdd("en-US,en;q=0.9");
    }
}
