using TikTokExplode.Exceptions;

namespace TikTokExplode.Publications.Soundtracks;

/// <summary>
/// Provides access to TikTok publication soundtrack metadata.
/// </summary>
public class SoundtrackClient
{
    private readonly TikTokApiClient _apiClient;

    internal SoundtrackClient(TikTokApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    /// <summary>
    /// Gets the soundtrack of a publication by its url.
    /// </summary>
    /// <param name="publicationUrl">TikTok publication url.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Soundtrack metadata.</returns>
    /// <exception cref="TikTokExplodeException">Thrown when the url is invalid or the request fails.</exception>
    public async Task<Soundtrack> GetAsync(string publicationUrl, CancellationToken cancellationToken = default)
    {
        string fullUrl = await UrlHelper.ResolveAndValidateAsync(publicationUrl, PublicationClient.PublicationType.NoMetter, cancellationToken).ConfigureAwait(false);
        string json = await _apiClient.GetApiResponseAsync(fullUrl, cancellationToken).ConfigureAwait(false);

        AwemeDto aweme = ApiResponseParser.ParseFirstAweme(json);
        return ApiResponseParser.ParseSoundtrack(aweme.Music);
    }
}
