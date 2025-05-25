using System.Text.Json;
using System.Text.RegularExpressions;
using TikTokExplode.Exceptions;

namespace TikTokExplode.Publications.Videos
{
	public partial class VideoClient
	{
	    private string ExtractVideoId(string url)
        {
            var match = Regex.Match(url, @"https:\/\/www\.tiktok\.com\/@[^/]+\/video\/(\d+)");
            return match.Success ? match.Groups[1].Value : null;
        }

        private string ExtractVideoUrl(string apiResponse)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(apiResponse);
                string videoUrl = doc.RootElement
                                .GetProperty("aweme_list")[0]
                                .GetProperty("video")
                                .GetProperty("play_addr")
                                .GetProperty("url_list")[0].GetString();

                return videoUrl?.Replace("\\u0026", "&");
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException($"Error parsing JSON response: {ex.Message}");
            }
        }

        private int ExtractVideoWidth(string apiResponse)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(apiResponse);
                int videoWidth = doc.RootElement
                                .GetProperty("aweme_list")[0]
                                .GetProperty("video")
                                .GetProperty("play_addr")
                                .GetProperty("width").GetInt32();

                return videoWidth;
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException($"Error parsing JSON response: {ex.Message}");
            }
        }

        private int ExtractVideoHeight(string apiResponse)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(apiResponse);
                int videoHeight = doc.RootElement
                                .GetProperty("aweme_list")[0]
                                .GetProperty("video")
                                .GetProperty("height").GetInt32();

                return videoHeight;
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException($"Error parsing JSON response: {ex.Message}");
            }
        }

        private ulong ExtractVideoDuration(string apiResponse)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(apiResponse);
                ulong videoDuration = doc.RootElement
                                .GetProperty("aweme_list")[0]
                                .GetProperty("video")
                                .GetProperty("duration").GetUInt64();

                return videoDuration;
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException($"Error parsing JSON response: {ex.Message}");
            }
        }
    }
}

