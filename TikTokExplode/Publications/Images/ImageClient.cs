using System.Threading;
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

        /// <summary>
        /// Gets Images list object by publication link
        /// </summary>
        public async Task<List<Image>> GetAsync(string publicationUrl, CancellationToken cancellationToken = default)
        {
            try
            { 
                if (!await _webRequestsHandler.IsUrlValidAsync(publicationUrl, PublicationClient.PublicationType.Images))
                    throw new TikTokExplodeException("Invalid URL");

                string apiResponse = await _webRequestsHandler.GetApiResponseAsync(publicationUrl, cancellationToken: cancellationToken);
                int imageCount = _apiExtractor.ExtractImagesCount(apiResponse);

                List<Image> images = new List<Image>(imageCount);

                for (int i = 0; i < imageCount; i++)
                {
                    images.Add(new Image
                    {
                        AwemeId = _apiExtractor.ExtractPublicationId(await _webRequestsHandler.GetFullUrlAsync(publicationUrl)),
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
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                throw new TikTokExplodeException($"Error retrieving images: {ex.Message}");
            }
        }
    }
}