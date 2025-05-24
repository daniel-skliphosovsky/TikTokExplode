using System.Net;
using System.Text.RegularExpressions;

namespace TikTokExplode.Publications
{
	public class PublicationClient
	{
		private HttpClient _httpClient;

		public PublicationClient(HttpClient httpClient)
		{
			_httpClient = httpClient;
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

