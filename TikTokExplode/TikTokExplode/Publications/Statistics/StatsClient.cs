using TikTokExplode.Exceptions;
using TikTokExplode.Extractors;
using TikTokExplode.WebRequester;

namespace TikTokExplode.Publications.Statistics
{
	public class StatsClient
	{
        public Stats GetAsync(string publicationUrl)
        {
            string fullUrl = WebRequestsHandler.GetFullUrl(publicationUrl).Result;

            if (!(WebRequestsHandler.IsUrlValid(fullUrl).Result && (fullUrl.Contains("/video/") || fullUrl.Contains("/photo/"))))
                throw new TikTokExplodeException("Invalid URL");

            string apiResponse = new WebRequestsHandler().GetApiResponse(fullUrl).Result;

            ApiExtractor apiExtractor = new ApiExtractor();
            Stats stats = new Stats()
            {
                CommentCount = apiExtractor.ExtractCommentCount(apiResponse),
                DownloadingCount = apiExtractor.ExtractDownloadCount(apiResponse),
                PlayCount = apiExtractor.ExtractPlayCount(apiResponse),
                ShareCount = apiExtractor.ExtractShareCount(apiResponse)
            };

            return stats;
        }
    }
}

