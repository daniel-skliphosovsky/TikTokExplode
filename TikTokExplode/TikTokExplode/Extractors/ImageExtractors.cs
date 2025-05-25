using System.Text.Json;
using TikTokExplode.Exceptions;

namespace TikTokExplode.Extractors
{
    public partial class ApiExtractor
    {
        public int ExtractImagesCount(string apiResponse)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(apiResponse);
                int imagesCount = doc.RootElement
                                .GetProperty("aweme_list")[0]
                                .GetProperty("image_post_info")
                                .GetProperty("images").GetArrayLength();

                return imagesCount;
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException($"Error parsing JSON response: {ex.Message}");
            }
        }

        public string ExtractImageUrl(string apiResponse, int imageIndex)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(apiResponse);
                string imageUrl = doc.RootElement
                                .GetProperty("aweme_list")[0]
                                .GetProperty("image_post_info")
                                .GetProperty("images")[imageIndex]
                                .GetProperty("display_image").
                                GetProperty("url_list")[1].GetString();

                return imageUrl;
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException($"Error parsing JSON response: {ex.Message}");
            }
        }
        public int ExtractImageWidth(string apiResponse, int imageIndex)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(apiResponse);
                int imageWidth = doc.RootElement
                                .GetProperty("aweme_list")[0]
                                .GetProperty("image_post_info")
                                .GetProperty("images")[imageIndex]
                                .GetProperty("display_image").
                                GetProperty("width").GetInt32();

                return imageWidth;
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException($"Error parsing JSON response: {ex.Message}");
            }
        }
        public int ExtractImageHeight(string apiResponse, int imageIndex)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(apiResponse);
                int imageHeight = doc.RootElement
                                .GetProperty("aweme_list")[0]
                                .GetProperty("image_post_info")
                                .GetProperty("images")[imageIndex]
                                .GetProperty("display_image").
                                GetProperty("height").GetInt32();

                return imageHeight;
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException($"Error parsing JSON response: {ex.Message}");
            }
        }
    }
}

