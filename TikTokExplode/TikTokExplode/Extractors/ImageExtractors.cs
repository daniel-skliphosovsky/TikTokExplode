using System.Text.Json;
using TikTokExplode.Exceptions;

namespace TikTokExplode.Extractors
{
    public partial class ApiExtractor
    {
        private static JsonElement GetImagePostInfoElement(string apiResponse)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(apiResponse);
                return doc.RootElement
                    .GetProperty("aweme_list")[0]
                    .GetProperty("image_post_info").Clone();
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException($"Error parsing JSON response: {ex.Message}");
            }
        }

        private static JsonElement GetImageElement(string apiResponse, int imageIndex)
        {
            JsonElement imagePostInfo = GetImagePostInfoElement(apiResponse);
            return imagePostInfo
                .GetProperty("images")[imageIndex]
                .GetProperty("display_image");
        }

        public int ExtractImagesCount(string apiResponse)
        {
            JsonElement imagePostInfo = GetImagePostInfoElement(apiResponse);
            return imagePostInfo
                .GetProperty("images")
                .GetArrayLength();
        }

        public string ExtractImageUrl(string apiResponse, int imageIndex)
        {
            JsonElement displayImage = GetImageElement(apiResponse, imageIndex);
            JsonElement urlList = displayImage.GetProperty("url_list");
            return urlList[urlList.GetArrayLength() - 1].GetString();
        }

        public int ExtractImageWidth(string apiResponse, int imageIndex)
        {
            JsonElement displayImage = GetImageElement(apiResponse, imageIndex);
            return displayImage.GetProperty("width").GetInt32();
        }

        public int ExtractImageHeight(string apiResponse, int imageIndex)
        {
            JsonElement displayImage = GetImageElement(apiResponse, imageIndex);
            return displayImage.GetProperty("height").GetInt32();
        }
    }
}