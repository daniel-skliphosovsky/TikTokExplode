using System.Net;

namespace TikTokExplode;

/// <summary>
/// Lightweight retry policy with exponential backoff and jitter.
/// Retries transient failures (HTTP 408/425/429/5xx and network errors).
/// Replaces the Polly-based pipeline that the library used before.
/// </summary>
internal sealed class HttpRetryHandler
{
    private const int MaxRetryDelayMs = 10_000;

    private readonly int _maxRetries;
    private readonly int _baseDelayMs;

    public HttpRetryHandler(int maxRetries, int baseDelayMs)
    {
        _maxRetries = maxRetries;
        _baseDelayMs = baseDelayMs;
    }

    public async Task<HttpResponseMessage> SendAsync(
        HttpClient client,
        Func<HttpRequestMessage> requestFactory,
        HttpCompletionOption completionOption,
        CancellationToken ct)
    {
        for (int attempt = 0; ; attempt++)
        {
            ct.ThrowIfCancellationRequested();

            using var request = requestFactory();

            HttpResponseMessage response;
            try
            {
                response = await client.SendAsync(request, completionOption, ct).ConfigureAwait(false);
            }
            catch (HttpRequestException)
            {
                if (attempt >= _maxRetries)
                    throw;

                await Task.Delay(GetRetryDelay(null, attempt), ct).ConfigureAwait(false);
                continue;
            }

            if (response.IsSuccessStatusCode || attempt >= _maxRetries || !IsRetryable(response))
                return response;

            response.Dispose();
            await Task.Delay(GetRetryDelay(response, attempt), ct).ConfigureAwait(false);
        }
    }

    private static bool IsRetryable(HttpResponseMessage response)
    {
        return response.StatusCode is HttpStatusCode.RequestTimeout
            or HttpStatusCode.TooManyRequests
            or >= HttpStatusCode.InternalServerError;
    }

    private TimeSpan GetRetryDelay(HttpResponseMessage? response, int attempt)
    {
        // Honor the Retry-After header when the API rate-limits us.
        if (response?.StatusCode == HttpStatusCode.TooManyRequests
            && response.Headers.TryGetValues("Retry-After", out var values)
            && int.TryParse(values.FirstOrDefault(), out int seconds))
        {
            return TimeSpan.FromSeconds(Math.Min(seconds, MaxRetryDelayMs / 1000));
        }

        double delay = _baseDelayMs * Math.Pow(2, attempt) + Random.Shared.Next(0, _baseDelayMs);
        return TimeSpan.FromMilliseconds(Math.Min(delay, MaxRetryDelayMs));
    }
}
