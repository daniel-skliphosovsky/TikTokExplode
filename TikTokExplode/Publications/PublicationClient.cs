using TikTokExplode.Exceptions;
using TikTokExplode.Extractors;
using TikTokExplode.WebRequester;
using TikTokExplode.Publications.Videos;
using TikTokExplode.Publications.Images;
using TikTokExplode.Publications.Authors;
using TikTokExplode.Publications.Soundtracks;
using TikTokExplode.Publications.Statistics;

namespace TikTokExplode.Publications
{
	public class PublicationClient
    {
        public VideoClient Videos { get; }

        public AuthorClient Authors { get; }

        public ImageClient Images { get; }

        public SoundtrackClient Soundtracks { get; }

        public StatsClient Statistics { get; }


        private readonly WebRequestsHandler _webRequestsHandler;
        private readonly ApiExtractor _apiExtractor;

        public PublicationClient()
        {
            Videos = new VideoClient();
            Authors = new AuthorClient();
            Images = new ImageClient();
            Soundtracks = new SoundtrackClient();
            Statistics = new StatsClient();

            _webRequestsHandler = new WebRequestsHandler();
            _apiExtractor = new ApiExtractor();
        }

        public enum PublicationType
        {
            Images,
            Video,
            NoMetter,
            Unknown
        }

        public static async Task<PublicationType> GetPublicationType(string url)
        {
            string fullUrl = await new WebRequestsHandler().GetFullUrlAsync(url);
            return fullUrl.Contains("/photo/") ? PublicationType.Images : fullUrl.Contains("/video/") ? PublicationType.Video : PublicationType.Unknown;
        }

        /// <summary>
        /// Gets Publication object by publication link (include Author, Stats, Music, Images/Video)
        /// </summary>
        public async Task<Publication> GetAsync(string publicationUrl, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!await _webRequestsHandler.IsUrlValidAsync(publicationUrl, PublicationType.NoMetter))
                    throw new TikTokExplodeException("Invalid URL");

                string apiResponse = await _webRequestsHandler.GetApiResponseAsync(publicationUrl, cancellationToken: cancellationToken);

                return new Publication
                {
                    Author = await new AuthorClient().GetAsync(publicationUrl, cancellationToken),
                    Images = await GetPublicationType(publicationUrl) == PublicationType.Images ? await new ImageClient().GetAsync(publicationUrl, cancellationToken) : null,
                    Video = await GetPublicationType(publicationUrl) == PublicationType.Video ? await new VideoClient().GetAsync(publicationUrl, cancellationToken) : null,
                    Soundtrack = await new SoundtrackClient().GetAsync(publicationUrl, cancellationToken),
                    Statistics = await new StatsClient().GetAsync(publicationUrl, cancellationToken),
                    Description = _apiExtractor.ExtractPublicationDescription(apiResponse),
                    IsAds = _apiExtractor.ExtractPublicationIsAdsStatus(apiResponse),
                    Id = _apiExtractor.ExtractPublicationId(await _webRequestsHandler.GetFullUrlAsync(publicationUrl))
                };
            }
            catch (TikTokExplodeException)
            {
                throw;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex) 
            {
                throw new TikTokExplodeException($"Error retrieving publication: {ex.Message}");
            }
        }
    }
}

