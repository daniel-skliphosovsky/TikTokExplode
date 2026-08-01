using TikTokExplode.Exceptions;

namespace TikTokExplode.Publications.Authors;

/// <summary>
/// Provides access to TikTok publication author metadata.
/// </summary>
public class AuthorClient
{
    private readonly TikTokApiClient _apiClient;

    internal AuthorClient(TikTokApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    /// <summary>
    /// Gets the author of a publication by its url.
    /// </summary>
    /// <param name="publicationUrl">TikTok publication url.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Author metadata.</returns>
    /// <exception cref="TikTokExplodeException">Thrown when the url is invalid or the request fails.</exception>
    public async Task<Author> GetAsync(string publicationUrl, CancellationToken cancellationToken = default)
    {
        string fullUrl = await UrlHelper.ResolveAndValidateAsync(publicationUrl, PublicationClient.PublicationType.NoMetter, cancellationToken).ConfigureAwait(false);
        string json = await _apiClient.GetApiResponseAsync(fullUrl, cancellationToken).ConfigureAwait(false);

        AwemeDto aweme = ApiResponseParser.ParseFirstAweme(json);
        return ApiResponseParser.ParseAuthor(aweme.Author);
    }
}
