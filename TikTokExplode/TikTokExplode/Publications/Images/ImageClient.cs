using TikTokExplode.Exceptions;
using TikTokExplode.Extractors;
using TikTokExplode.WebRequester;

namespace TikTokExplode.Publications.Images
{
    public class ImageClient
    {
        private readonly WebRequestsHandler _webRequestsHandler;
        private readonly ApiExtractor _apiExtractor;

        public ImageClient()
        {
            _webRequestsHandler = new WebRequestsHandler();
            _apiExtractor = new ApiExtractor();
        }

        public async Task<List<Image>> GetAsync(string publicationUrl)
        {
            try
            {
                string fullUrl = await _webRequestsHandler.GetFullUrl(publicationUrl);

                if (!await _webRequestsHandler.IsUrlValid(fullUrl, PublicationClient.PublicationType.Images))
                    throw new TikTokExplodeException("Invalid URL");

                string apiResponse = await _webRequestsHandler.GetApiResponse(fullUrl);
                int imageCount = _apiExtractor.ExtractImagesCount(apiResponse);

                List<Image> images = new List<Image>(imageCount);

                for (int i = 0; i < imageCount; i++)
                {
                    images.Add(new Image
                    {
                        Url = _apiExtractor.ExtractImageUrl(apiResponse, i),
                        Width = _apiExtractor.ExtractImageWidth(apiResponse, i),
                        Height = _apiExtractor.ExtractImageHeight(apiResponse, i)
                    });
                }

                return images;
            }
            catch (TikTokExplodeException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException($"Error retrieving images: {ex.Message}");
            }
        }
    }
}