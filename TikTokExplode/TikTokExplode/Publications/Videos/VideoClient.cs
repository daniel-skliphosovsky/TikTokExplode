using System.Net.Http;
using System.Text.RegularExpressions;

namespace TikTokExplode.Publications.Videos
{
	public class VideoClient
	{
		public Video GetAsync(string videoUrl)
		{
			Video video = new Video();

            HttpClient httpClient = new HttpClient();

            return video;
		}

        private string ExtractVideoId(string url)
        {
            var match = Regex.Match(url, @"https:\/\/www\.tiktok\.com\/@[^/]+\/video\/(\d+)");
            return match.Success ? match.Groups[1].Value : null;
        }
    }
}

