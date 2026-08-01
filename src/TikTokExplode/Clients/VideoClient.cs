using TikTokExplode.Exceptions;

namespace TikTokExplode.Publications.Videos;

/// <summary>
/// Provides access to TikTok publication video metadata.
/// </summary>
public class VideoClient
{
    private readonly TikTokApiClient _apiClient;

    internal VideoClient(TikTokApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    /// <summary>
    /// Gets the video of a publication by its url.
    /// </summary>
    /// <param name="publicationUrl">TikTok publication url.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Video metadata.</returns>
    /// <exception cref="TikTokExplodeException">Thrown when the url is invalid or the request fails.</exception>
    public async Task<Video> GetAsync(string publicationUrl, CancellationToken cancellationToken = default)
    {
        string fullUrl = await UrlHelper.ResolveAndValidateAsync(publicationUrl, PublicationClient.PublicationType.Video, cancellationToken).ConfigureAwait(false);
        string json = await _apiClient.GetApiResponseAsync(fullUrl, cancellationToken).ConfigureAwait(false);

        AwemeDto aweme = ApiResponseParser.ParseFirstAweme(json);
        return ApiResponseParser.ParseVideo(aweme.Video, aweme.AwemeId)
            ?? throw new ValidationException("Failed to extract video data from response.");
    }
}
