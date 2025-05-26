using System.Text.Json;
using TikTokExplode.Exceptions;

namespace TikTokExplode.Extractors
{
	public partial class ApiExtractor
	{
        public ulong ExtractCommentCount(string apiResponse)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(apiResponse);
                ulong count = doc.RootElement
                                .GetProperty("aweme_list")[0]
                                .GetProperty("statistics")
                                .GetProperty("comment_count").GetUInt64();

                return count;
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException($"Error parsing JSON response: {ex.Message}");
            }
        }
        public ulong ExtractDownloadCount(string apiResponse)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(apiResponse);
                ulong count = doc.RootElement
                                .GetProperty("aweme_list")[0]
                                .GetProperty("statistics")
                                .GetProperty("download_count").GetUInt64();

                return count;
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException($"Error parsing JSON response: {ex.Message}");
            }
        }
        public ulong ExtractPlayCount(string apiResponse)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(apiResponse);
                ulong count = doc.RootElement
                                .GetProperty("aweme_list")[0]
                                .GetProperty("statistics")
                                .GetProperty("play_count").GetUInt64();

                return count;
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException($"Error parsing JSON response: {ex.Message}");
            }
        }
        public ulong ExtractShareCount(string apiResponse)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(apiResponse);
                ulong count = doc.RootElement
                                .GetProperty("aweme_list")[0]
                                .GetProperty("statistics")
                                .GetProperty("share_count").GetUInt64();

                return count;
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException($"Error parsing JSON response: {ex.Message}");
            }
        }
    }
}

