using System.Net;
using System.Text.RegularExpressions;
using TikTokExplode.Exceptions;
using TikTokExplode.Publications;

namespace TikTokExplode.WebRequester
{
	public partial class WebRequestsHandler
	{
        /// <summary>
        /// Gets full url of TikTok publication
        /// </summary>
        public async Task<string> GetFullUrlAsync(string url)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(url))
                    throw new TikTokExplodeException("Publication URL cannot be null or empty");

                HttpWebRequest request = WebRequest.CreateHttp(url);
                request.AllowAutoRedirect = true;
                WebResponse response = await request.GetResponseAsync();
                return response.ResponseUri.AbsoluteUri ?? null;
            }
            catch(TikTokExplodeException)
            {
                throw;
            }
            catch(Exception ex)
            {
                throw new TikTokExplodeException("GetFullPath() exception: " + ex);
            }
        }

        /// <summary>
        /// Checks for url is correct 
        /// </summary>
        public async Task<bool> IsUrlValidAsync(string url, PublicationClient.PublicationType publicationType)
        {
            string fullUrl = await GetFullUrlAsync(url);
            if (Regex.IsMatch(fullUrl, @"https:\/\/www\.tiktok\.com\/.+"))
            {
                switch (publicationType)
                {
                    case PublicationClient.PublicationType.Video:
                        if (await PublicationClient.GetPublicationType(fullUrl) == PublicationClient.PublicationType.Video)
                            return true;
                        else return false;
                    case PublicationClient.PublicationType.Images:
                        if (await PublicationClient.GetPublicationType(fullUrl) == PublicationClient.PublicationType.Images)
                            return true;
                        else return false;
                    case PublicationClient.PublicationType.NoMetter:
                        if (await PublicationClient.GetPublicationType(fullUrl) == PublicationClient.PublicationType.Images ||
                            await PublicationClient.GetPublicationType(fullUrl) == PublicationClient.PublicationType.Video)
                            return true;
                        else return false;
                    default:
                        return false;
                }   
            }
            else
            {
                return false;
            }
        }

    }
}

