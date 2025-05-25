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
                string videoWidth = doc.RootElement
                                .GetProperty("aweme_list")[0]
                                .GetProperty("video")
                                .GetProperty("play_addr")
                                .GetProperty("width").GetString();

                return int.TryParse(videoWidth, out int width) ? int.Parse(videoWidth) : 0;
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
                string videoHeight = doc.RootElement
                                .GetProperty("aweme_list")[0]
                                .GetProperty("video")
                                .GetProperty("height").GetString();

                return int.TryParse(videoHeight, out int height) ? int.Parse(videoHeight) : 0;
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
                string videoDuration = doc.RootElement
                                .GetProperty("aweme_list")[0]
                                .GetProperty("video")
                                .GetProperty("duration").GetString();

                return ulong.TryParse(videoDuration, out ulong duration) ? ulong.Parse(videoDuration) : 0;
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException($"Error parsing JSON response: {ex.Message}");
            }
        }
    }
}

