using TikTokExplode.Exceptions;
using TikTokExplode.WebRequester;

namespace TikTokExplode.Publications.Videos
{
	public partial class VideoClient
	{
		public Video GetAsync(string videoUrl)
		{
            string fullUrl = WebRequestsHandler.GetFullUrl(videoUrl).Result;

            if (!WebRequestsHandler.IsUrlValid(fullUrl).Result && fullUrl.Contains("/video/"))
                throw new TikTokExplodeException("Invalid URL");

            string videoId = ExtractVideoId(fullUrl);

            if (string.IsNullOrEmpty(videoId))
                throw new TikTokExplodeException("Failed to extract video ID");

            string apiResponse = new WebRequestsHandler().GetApiResponse(videoId).Result;

            if (string.IsNullOrEmpty(apiResponse))
                throw new TikTokExplodeException("API request return failure Status Code");

            Video video = new Video()
            {
                Url = ExtractVideoUrl(apiResponse),
                Width = ExtractVideoWidth(apiResponse),
                Height = ExtractVideoHeight(apiResponse),
                Duration = ExtractVideoDuration(apiResponse)
            };

            return video;
		}
    }
}

