using System.Net.Http.Headers;

namespace TikTokExplode.Infrastructure.Http;

public sealed class HeadersProvider
{
    private readonly string[] _userAgents;

    public HeadersProvider(string[] userAgents)
    {
        _userAgents = userAgents;
    }

    /// <summary>
    /// Applies randomized user-agent and standard HTTP headers to the given request headers.
    /// </summary>
    /// <param name="headers">The request headers to apply values to.</param>
    public void ApplyHeaders(HttpRequestHeaders headers)
    {
        var rng = Random.Shared;
        var ua = _userAgents[rng.Next(_userAgents.Length)];
        headers.UserAgent.ParseAdd(ua);
        headers.Accept.ParseAdd("application/json, text/plain, */*");
        headers.AcceptLanguage.ParseAdd("en-US,en;q=0.9");
    }
}
