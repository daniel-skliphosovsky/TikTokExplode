using TikTokExplode.Exceptions;
using TikTokExplode.Extractors;
using TikTokExplode.WebRequester;
using TikTokExplode.Publications.Videos;
using TikTokExplode.Publications.Images;
using TikTokExplode.Publications.Authors;
using TikTokExplode.Publications.Musics;
using TikTokExplode.Publications.Statistics;

namespace TikTokExplode.Publications
{
	public class PublicationClient
    {
        public VideoClient Videos { get; }

        public AuthorClient Authors { get; }

        public ImageClient Images { get; }

        public MusicClient Musics { get; }

        public StatsClient Statistics { get; }


        private readonly WebRequestsHandler _webRequestsHandler;
        private readonly ApiExtractor _apiExtractor;

        public PublicationClient()
        {
            Videos = new VideoClient();
            Authors = new AuthorClient();
            Images = new ImageClient();
            Musics = new MusicClient();
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

        public PublicationType GetPublicationType(string url)
        {
            string fullurl = _webRequestsHandler.GetFullUrl(url).Result;
            return fullurl.Contains("/photo/") ? PublicationType.Images : fullurl.Contains("/video/") ? PublicationType.Video : PublicationType.Unknown;
        }

        public async Task<Publication> GetAsync(string publicationUrl)
        {
            try
            {
                string fullUrl = await _webRequestsHandler.GetFullUrl(publicationUrl);

                if (!(await _webRequestsHandler.IsUrlValid(fullUrl) && (fullUrl.Contains("/video/") || fullUrl.Contains("/photo/"))))
                    throw new TikTokExplodeException("Invalid URL");

                string apiResponse = await _webRequestsHandler.GetApiResponse(fullUrl);

                return new Publication
                {
                    Author = await new AuthorClient().GetAsync(publicationUrl),
                    Images = fullUrl.Contains("/photo/") ? await new ImageClient().GetAsync(publicationUrl) : null,
                    Video = fullUrl.Contains("/video/") ? await new VideoClient().GetAsync(publicationUrl) : null,
                    Music = await new MusicClient().GetAsync(publicationUrl),
                    Statistics = await new StatsClient().GetAsync(publicationUrl)
                };
            }
            catch (TikTokExplodeException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException($"Error retrieving author: {ex.Message}");
            }
        }
    }
}

