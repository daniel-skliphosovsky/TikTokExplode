using TikTokExplode.Exceptions;
using TikTokExplode.Extractors;
using TikTokExplode.WebRequester;

namespace TikTokExplode.Publications.Musics
{
	public class MusicClient
	{
        public Music GetAsync(string publicationUrl)
        {
            string fullUrl = WebRequestsHandler.GetFullUrl(publicationUrl).Result;

            if (!(WebRequestsHandler.IsUrlValid(fullUrl).Result && (fullUrl.Contains("/video/") || fullUrl.Contains("/photo/"))))
                throw new TikTokExplodeException("Invalid URL");

            string apiResponse = new WebRequestsHandler().GetApiResponse(fullUrl).Result;

            ApiExtractor apiExtractor = new ApiExtractor();
            Music video = new Music()
            {
                Url = apiExtractor.ExtractVideoUrl(apiResponse),
                Width = apiExtractor.ExtractVideoWidth(apiResponse),
                Height = apiExtractor.ExtractVideoHeight(apiResponse),
                Duration = apiExtractor.ExtractVideoDuration(apiResponse)
            };

            return Music;
        }
    }
}

