using System.Text.Json;
using System.Text.RegularExpressions;
using TikTokExplode.Exceptions;

namespace TikTokExplode.Extractors
{
	public partial class ApiExtractor
	{
	    public string ExtractPublicationId(string url)
        {
            var match = Regex.Match(url, @"https:\/\/www\.tiktok\.com\/@[^/]+\/(video|photo)\/(\d+)");
            return match.Success ? match.Groups[1].Value : null;
        }

        private static JsonElement GetPublicationElement(string apiResponse)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(apiResponse);
                return doc.RootElement
                    .GetProperty("aweme_list")[0];
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException($"Error parsing JSON response: {ex.Message}");
            }
        }

        public string ExtractPublicationDescription(string apiResponse)
        {
            JsonElement publication = GetPublicationElement(apiResponse);
            return publication
                .GetProperty("desc")
                .GetString();
        }

        public bool ExtractPublicationIsAdsStatus(string apiResponse)
        {
            JsonElement publication = GetPublicationElement(apiResponse);
            return publication
                .GetProperty("is_ads")
                .GetBoolean();
        }
    }
}

