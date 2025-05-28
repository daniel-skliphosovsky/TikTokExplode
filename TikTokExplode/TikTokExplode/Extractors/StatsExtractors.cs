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

        /// <summary>
        /// Extracts Publication CommentCount from api response
        /// </summary>
        public ulong ExtractCommentCount(string apiResponse) =>
            ExtractStatisticCount(apiResponse, "comment_count");

        /// <summary>
        /// Extracts Publication DownlaodCount from api response
        /// </summary>
        public ulong ExtractDownloadCount(string apiResponse) =>
            ExtractStatisticCount(apiResponse, "download_count");

        /// <summary>
        /// Extracts Publication PlayCount from api response
        /// </summary>
        public ulong ExtractPlayCount(string apiResponse) =>
            ExtractStatisticCount(apiResponse, "play_count");

        /// <summary>
        /// Extracts Publication ShareCount from api response
        /// </summary>
        public ulong ExtractShareCount(string apiResponse) =>
            ExtractStatisticCount(apiResponse, "share_count");

        /// <summary>
        /// Extracts Publication DiggCount from api response
        /// </summary>
        public ulong ExtractDiggCount(string apiResponse) =>
            ExtractStatisticCount(apiResponse, "digg_count");

        /// <summary>
        /// Extracts Publication ForwardCount from api response
        /// </summary>
        public ulong ExtractForwardCount(string apiResponse) =>
            ExtractStatisticCount(apiResponse, "forward_count");

        /// <summary>
        /// Extracts Publication RepostCount from api response
        /// </summary>
        public ulong ExtractRepostCount(string apiResponse) =>
            ExtractStatisticCount(apiResponse, "repost_count");
    }
}