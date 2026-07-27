using System.Net;

namespace TikTokExplode.Infrastructure.Http;

public sealed class RateLimitHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            var retryAfter = GetRetryAfter(response.Headers);
            if (retryAfter.HasValue)
            {
                response.Dispose();
                await Task.Delay(retryAfter.Value, cancellationToken);
                return await base.SendAsync(request, cancellationToken);
            }
        }

        return response;
    }

    private static TimeSpan? GetRetryAfter(System.Net.Http.Headers.HttpResponseHeaders headers)
    {
        if (headers.TryGetValues("Retry-After", out var values))
        {
            var retryAfterHeader = values.FirstOrDefault();
            if (int.TryParse(retryAfterHeader, out var seconds))
                return TimeSpan.FromSeconds(seconds);
            if (DateTimeOffset.TryParse(retryAfterHeader, out var date))
                return date - DateTimeOffset.UtcNow;
        }
        return null;
    }
}
