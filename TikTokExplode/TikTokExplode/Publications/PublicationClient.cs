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

        public async Task<PublicationType> GetPublicationType(string url)
        {
            string fullurl = await _webRequestsHandler.GetFullUrlAsync(url);
            return fullurl.Contains("/photo/") ? PublicationType.Images : fullurl.Contains("/video/") ? PublicationType.Video : PublicationType.Unknown;
        }

        /// <summary>
        /// Gets Publication object by publication link (include Author, Stats, Music, Images/Video)
        /// </summary>
        public async Task<Publication> GetAsync(string publicationUrl)
        {
            try
            {
                string fullUrl = await _webRequestsHandler.GetFullUrlAsync(publicationUrl);

                if (!await _webRequestsHandler.IsUrlValidAsync(fullUrl, PublicationType.NoMetter))
                    throw new TikTokExplodeException("Invalid URL");

                string apiResponse = await _webRequestsHandler.GetApiResponseAsync(fullUrl);

                return new Publication
                {
                    Author = await new AuthorClient().GetAsync(publicationUrl),
                    Images = await GetPublicationType(publicationUrl) == PublicationType.Images ? await new ImageClient().GetAsync(publicationUrl) : null,
                    Video = await GetPublicationType(publicationUrl) == PublicationType.Video ? await new VideoClient().GetAsync(publicationUrl) : null,
                    Soundtrack = await new SoundtrackClient().GetAsync(publicationUrl),
                    Statistics = await new StatsClient().GetAsync(publicationUrl),
                    Description = _apiExtractor.ExtractPublicationDescription(apiResponse),
                    IsAds = _apiExtractor.ExtractPublicationIsAdsStatus(apiResponse),
                    Id = _apiExtractor.ExtractPublicationId(fullUrl)
                };
            }
            catch (TikTokExplodeException)
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

