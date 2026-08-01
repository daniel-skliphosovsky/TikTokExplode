using TikTokExplode.Exceptions;
using TikTokExplode.Publications.Authors;
using TikTokExplode.Publications.Images;
using TikTokExplode.Publications.Soundtracks;
using TikTokExplode.Publications.Statistics;
using TikTokExplode.Publications.Videos;

namespace TikTokExplode.Publications;

/// <summary>
/// Provides access to TikTok publications and their components.
/// </summary>
public class PublicationClient
{
    /// <summary>
    /// Type of a TikTok publication.
    /// </summary>
    public enum PublicationType
    {
        Images,
        Video,
        NoMetter,
        Unknown
    }

    /// <summary>
    /// Publication author.
    /// </summary>
    public AuthorClient Authors { get; }

    /// <summary>
    /// Publication statistics.
    /// </summary>
    public StatsClient Statistics { get; }

    /// <summary>
    /// Publication soundtrack.
    /// </summary>
    public SoundtrackClient Soundtracks { get; }

    /// <summary>
    /// Publication video.
    /// </summary>
    public VideoClient Videos { get; }

    /// <summary>
    /// Publication images.
    /// </summary>
    public ImageClient Images { get; }

    private readonly TikTokApiClient _apiClient;

    internal PublicationClient(TikTokApiClient apiClient)
    {
        _apiClient = apiClient;
        Authors = new AuthorClient(apiClient);
        Statistics = new StatsClient(apiClient);
        Soundtracks = new SoundtrackClient(apiClient);
        Videos = new VideoClient(apiClient);
        Images = new ImageClient(apiClient);
    }

    /// <summary>
    /// Determines the type of a TikTok publication by its url.
    /// </summary>
    /// <param name="url">TikTok publication url (short links are supported).</param>
    /// <returns>The publication type, or <see cref="PublicationType.Unknown"/> if the url is not a publication.</returns>
    public static async Task<PublicationType> GetPublicationType(string url)
    {
        string fullUrl = await UrlHelper.ResolveAsync(url).ConfigureAwait(false);
        return GetPublicationTypeFromUrl(fullUrl);
    }

    internal static PublicationType GetPublicationTypeFromUrl(string fullUrl)
    {
        if (fullUrl.Contains("/photo/", StringComparison.OrdinalIgnoreCase))
            return PublicationType.Images;

        if (fullUrl.Contains("/video/", StringComparison.OrdinalIgnoreCase))
            return PublicationType.Video;

        return PublicationType.Unknown;
    }

    /// <summary>
    /// Gets a publication by its url. Includes author, statistics, soundtrack and media.
    /// </summary>
    /// <param name="publicationUrl">TikTok publication url.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Publication metadata.</returns>
    /// <exception cref="TikTokExplodeException">Thrown when the url is invalid or the request fails.</exception>
    public async Task<Publication> GetAsync(string publicationUrl, CancellationToken cancellationToken = default)
    {
        string fullUrl = await UrlHelper.ResolveAndValidateAsync(publicationUrl, PublicationType.NoMetter, cancellationToken).ConfigureAwait(false);
        string json = await _apiClient.GetApiResponseAsync(fullUrl, cancellationToken).ConfigureAwait(false);

        AwemeDto aweme = ApiResponseParser.ParseFirstAweme(json);
        return ApiResponseParser.ParsePublication(aweme, GetPublicationTypeFromUrl(fullUrl));
    }
}
