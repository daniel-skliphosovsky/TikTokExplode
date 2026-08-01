using TikTokExplode.Exceptions;

namespace TikTokExplode.Publications.Images;

/// <summary>
/// Provides access to TikTok publication image metadata.
/// </summary>
public class ImageClient
{
    private readonly TikTokApiClient _apiClient;

    internal ImageClient(TikTokApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    /// <summary>
    /// Gets the images of a publication by its url.
    /// </summary>
    /// <param name="publicationUrl">TikTok publication url.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of image metadata.</returns>
    /// <exception cref="TikTokExplodeException">Thrown when the url is invalid or the request fails.</exception>
    public async Task<List<Image>> GetAsync(string publicationUrl, CancellationToken cancellationToken = default)
    {
        string fullUrl = await UrlHelper.ResolveAndValidateAsync(publicationUrl, PublicationClient.PublicationType.Images, cancellationToken).ConfigureAwait(false);
        string json = await _apiClient.GetApiResponseAsync(fullUrl, cancellationToken).ConfigureAwait(false);

        AwemeDto aweme = ApiResponseParser.ParseFirstAweme(json);
        return ApiResponseParser.ParseImages(aweme.ImagePostInfo, aweme.AwemeId)
            ?? throw new ValidationException("Failed to extract images data from response.");
    }
}
