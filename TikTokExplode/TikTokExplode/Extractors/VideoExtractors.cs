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
                    .GetProperty("video");
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException($"Error parsing JSON response: {ex.Message}");
            }
        }

        public string ExtractVideoUrl(string apiResponse)
        {
            JsonElement video = GetVideoElement(apiResponse);
            string url = video
                .GetProperty("play_addr")
                .GetProperty("url_list")[0]
                .GetString();

            return url?.Replace("\\u0026", "&");
        }

        public int ExtractVideoWidth(string apiResponse)
        {
            JsonElement video = GetVideoElement(apiResponse);
            return video
                .GetProperty("width")
                .GetInt32();
        }

        public int ExtractVideoHeight(string apiResponse)
        {
            JsonElement video = GetVideoElement(apiResponse);
            return video
                .GetProperty("height")
                .GetInt32();
        }

        public ulong ExtractVideoDuration(string apiResponse)
        {
            JsonElement video = GetVideoElement(apiResponse);
            return video
                .GetProperty("duration")
                .GetUInt64();
        }
    }
}