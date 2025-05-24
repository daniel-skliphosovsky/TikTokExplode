using System;
using System.Net;
using System.Text.RegularExpressions;
using TikTokExplode.Exceptions;

namespace TikTokExplode.Publications
{
	public class PublicationClient
	{
		private HttpClient _httpClient;

		public PublicationClient(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task GetVideoAsync(string publicationUrl)
		{
            string fullUrl = await GetFullUrl(publicationUrl);

            if (!await IsUrlValid(fullUrl))
				throw new TikTokExplodeException("Invalid URL");

            string videoId = ExtractVideoId(fullUrl);

            if (string.IsNullOrEmpty(videoId))
                throw new TikTokExplodeException("Failed to extract video ID");
        }

        private string ExtractVideoId(string url)
        {
            var match = Regex.Match(url, @"https:\/\/www\.tiktok\.com\/@[^/]+\/video\/(\d+)");
			return match.Success ? match.Groups[1].Value : null;
        }

        private async Task<bool> IsUrlValid(string url)
		{
			return Regex.IsMatch(url, @"https:\/\/www\.tiktok\.com\/.+");
        }

		private async Task<string> GetFullUrl(string url)
		{
			try
			{
				HttpWebRequest request = WebRequest.CreateHttp(url);
				request.AllowAutoRedirect = true;
				WebResponse response = await request.GetResponseAsync();
				return response.ResponseUri.AbsoluteUri ?? null;
			}
			catch
			{
				return null;
			}
        }
	}
}

