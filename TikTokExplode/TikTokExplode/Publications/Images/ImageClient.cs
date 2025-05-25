using TikTokExplode.Exceptions;
using TikTokExplode.Extractors;
using TikTokExplode.WebRequester;

namespace TikTokExplode.Publications.Images
{
	public class ImageClient
	{
        public List<Image> GetAsync(string publicationUrl)
        {
            string fullUrl = WebRequestsHandler.GetFullUrl(publicationUrl).Result;

            if (!(WebRequestsHandler.IsUrlValid(fullUrl).Result && fullUrl.Contains("/photo/")))
                throw new TikTokExplodeException("Invalid URL");

            string apiResponse = new WebRequestsHandler().GetApiResponse(fullUrl).Result;

            ApiExtractor apiExtractor = new ApiExtractor();

            List<Image> images = new List<Image>();

            for (int i = 0; i < apiExtractor.ExtractImagesCount(apiResponse); i++)
            {
                Image image = new Image()
                {
                    Url = apiExtractor.ExtractImageUrl(apiResponse, i),
                    Width = apiExtractor.ExtractImageWidth(apiResponse, i),
                    Height = apiExtractor.ExtractImageHeight(apiResponse, i)
                };

                images.Add(image);
            }          

            return images;
        }
    }
}

