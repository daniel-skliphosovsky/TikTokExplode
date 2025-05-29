using System.Text.Json;
using System.Text.RegularExpressions;
using TikTokExplode.Exceptions;
using TikTokExplode.WebRequester;

namespace TikTokExplode.Extractors
{
	public partial class ApiExtractor
	{
        /// <summary>
        /// Extracts Publication aweme id from url
        /// </summary>
        public string ExtractPublicationId(string url)
        {
            Match match = Regex.Match(url, @"https:\/\/www\.tiktok\.com\/@[^/]+\/(video|photo)\/(\d+)");
            return match.Success ? match.Groups[2].Value : null;
        }

        private static JsonElement GetPublicationElement(string apiResponse)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(apiResponse);
                return doc.RootElement
                    .GetProperty("aweme_list")[0].Clone();
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException($"Error parsing JSON response: {ex.Message}");
            }
        }

        /// <summary>
        /// Extracts Publication Description from api response
        /// </summary>
        public string ExtractPublicationDescription(string apiResponse)
        {
            JsonElement publication = GetPublicationElement(apiResponse);
            return publication
                .GetProperty("desc")
                .GetString();
        }

        /// <summary>
        /// Extracts Publication IsAds Status from api response
        /// </summary>
        public bool ExtractPublicationIsAdsStatus(string apiResponse)
        {
            JsonElement publication = GetPublicationElement(apiResponse);
            return publication
                .GetProperty("is_ads")
                .GetBoolean();
        }
    }
}

