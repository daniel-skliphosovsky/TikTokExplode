using System.Text.Json;
using TikTokExplode.Exceptions;

namespace TikTokExplode.Extractors
{
    public partial class ApiExtractor
    {
        private static JsonElement GetVideoElement(string apiResponse)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(apiResponse);
                return doc.RootElement
                    .GetProperty("aweme_list")[0]
                    .GetProperty("video").Clone();
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException($"Error parsing JSON response: {ex.Message}");
            }
        }

        /// <summary>
        /// Extracts Video Url from api response
        /// </summary>
        public string ExtractVideoUrl(string apiResponse)
        {
            JsonElement video = GetVideoElement(apiResponse);
            string url = video
                .GetProperty("play_addr")
                .GetProperty("url_list")[0]
                .GetString();

            return url?.Replace("\\u0026", "&");
        }

        /// <summary>
        /// Extracts Video Width from api response
        /// </summary>
        public int ExtractVideoWidth(string apiResponse)
        {
            JsonElement video = GetVideoElement(apiResponse);
            return video
                .GetProperty("width")
                .GetInt32();
        }

        /// <summary>
        /// Extracts Video Height from api response
        /// </summary>
        public int ExtractVideoHeight(string apiResponse)
        {
            JsonElement video = GetVideoElement(apiResponse);
            return video
                .GetProperty("height")
                .GetInt32();
        }

        /// <summary>
        /// Extracts Video Duration from api response
        /// </summary>
        public ulong ExtractVideoDuration(string apiResponse)
        {
            JsonElement video = GetVideoElement(apiResponse);
            return video
                .GetProperty("duration")
                .GetUInt64();
        }
    }
}