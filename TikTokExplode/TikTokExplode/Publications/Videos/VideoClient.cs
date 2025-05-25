using TikTokExplode.Exceptions;
using TikTokExplode.Extractors;
using TikTokExplode.WebRequester;

namespace TikTokExplode.Publications.Videos
{
	public partial class VideoClient
	{
		public Video GetAsync(string publicationUrl)
		{
            string fullUrl = WebRequestsHandler.GetFullUrl(publicationUrl).Result;

            if (!WebRequestsHandler.IsUrlValid(fullUrl).Result && fullUrl.Contains("/video/"))
                throw new TikTokExplodeException("Invalid URL");

            string apiResponse = new WebRequestsHandler().GetApiResponse(fullUrl).Result;

            ApiExtractor apiExtractor = new ApiExtractor();
            Video video = new Video()
            {
                Url = apiExtractor.ExtractVideoUrl(apiResponse),
                Width = apiExtractor.ExtractVideoWidth(apiResponse),
                Height = apiExtractor.ExtractVideoHeight(apiResponse),
                Duration = apiExtractor.ExtractVideoDuration(apiResponse)
            };

            return video;
		}
    }
}

