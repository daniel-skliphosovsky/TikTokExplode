using System.Net;
using System.Text.RegularExpressions;

namespace TikTokExplode.WebRequester
{
	public partial class WebRequestsHandler
	{
        public async Task<string> GetFullUrl(string url)
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

        public async Task<bool> IsUrlValid(string url)
        {
            return Regex.IsMatch(url, @"https:\/\/www\.tiktok\.com\/.+");
        }
    }
}

