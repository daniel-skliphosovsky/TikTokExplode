using System.Text.Json;
using TikTokExplode.Exceptions;

namespace TikTokExplode.Extractors
{
    public partial class ApiExtractor
    {
        private static JsonElement GetStatisticsElement(string apiResponse)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(apiResponse);
                return doc.RootElement
                    .GetProperty("aweme_list")[0]
                    .GetProperty("statistics").Clone();
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException($"Error parsing JSON response: {ex.Message}");
            }
        }

        private static ulong ExtractStatisticCount(string apiResponse, string propertyName)
        {
            JsonElement statistics = GetStatisticsElement(apiResponse);
            return statistics.GetProperty(propertyName).GetUInt64();
        }

        public ulong ExtractCommentCount(string apiResponse) =>
            ExtractStatisticCount(apiResponse, "comment_count");

        public ulong ExtractDownloadCount(string apiResponse) =>
            ExtractStatisticCount(apiResponse, "download_count");

        public ulong ExtractPlayCount(string apiResponse) =>
            ExtractStatisticCount(apiResponse, "play_count");

        public ulong ExtractShareCount(string apiResponse) =>
            ExtractStatisticCount(apiResponse, "share_count");

        public ulong ExtractDiggCount(string apiResponse) =>
            ExtractStatisticCount(apiResponse, "digg_count");

        public ulong ExtractForwardCount(string apiResponse) =>
            ExtractStatisticCount(apiResponse, "forward_count");

        public ulong ExtractRepostCount(string apiResponse) =>
            ExtractStatisticCount(apiResponse, "repost_count");
    }
}