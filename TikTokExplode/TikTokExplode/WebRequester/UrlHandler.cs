using System.Net;
using System.Text.RegularExpressions;
using TikTokExplode.Exceptions;
using TikTokExplode.Publications;

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

        public async Task<bool> IsUrlValid(string fullUrl, PublicationClient.PublicationType publicationType)
        {
            if (Regex.IsMatch(fullUrl, @"https:\/\/www\.tiktok\.com\/.+"))
            {
                PublicationClient publicationClient = new PublicationClient();

                switch (publicationType)
                {
                    case PublicationClient.PublicationType.Video:
                        if (publicationClient.GetPublicationType(fullUrl) == PublicationClient.PublicationType.Video)
                            return true;
                        else return false;
                    case PublicationClient.PublicationType.Images:
                        if (publicationClient.GetPublicationType(fullUrl) == PublicationClient.PublicationType.Images)
                            return true;
                        else return false;
                    case PublicationClient.PublicationType.NoMetter:
                        if (publicationClient.GetPublicationType(fullUrl) == PublicationClient.PublicationType.Images ||
                            publicationClient.GetPublicationType(fullUrl) == PublicationClient.PublicationType.Video)
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

