using TikTokExplode.Exceptions;

namespace TikTokExplode.Publications.Statistics;

/// <summary>
/// Provides access to TikTok publication statistics.
/// </summary>
public class StatsClient
{
    private readonly TikTokApiClient _apiClient;

    internal StatsClient(TikTokApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    /// <summary>
    /// Gets the statistics of a publication by its url.
    /// </summary>
    /// <param name="publicationUrl">TikTok publication url.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Publication statistics.</returns>
    /// <exception cref="TikTokExplodeException">Thrown when the url is invalid or the request fails.</exception>
    public async Task<Stats> GetAsync(string publicationUrl, CancellationToken cancellationToken = default)
    {
        string fullUrl = await UrlHelper.ResolveAndValidateAsync(publicationUrl, PublicationClient.PublicationType.NoMetter, cancellationToken).ConfigureAwait(false);
        string json = await _apiClient.GetApiResponseAsync(fullUrl, cancellationToken).ConfigureAwait(false);

        AwemeDto aweme = ApiResponseParser.ParseFirstAweme(json);
        return ApiResponseParser.ParseStats(aweme.Statistics);
    }
}
