using System.Text.Json;
using TikTokExplode.Exceptions;

namespace TikTokExplode.Extractors
{
    public partial class ApiExtractor
    {
        private static JsonElement GetAuthorElement(string apiResponse)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(apiResponse);
                return doc.RootElement
                    .GetProperty("aweme_list")[0]
                    .GetProperty("author").Clone();
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException($"Error parsing JSON response: {ex.Message}");
            }
        }

        /// <summary>
        /// Extracts Author UserId from api response
        /// </summary>
        public string ExtractAuthorUserId(string apiResponse)
        {
            JsonElement author = GetAuthorElement(apiResponse);
            return author.GetProperty("uid").GetString();
        }

        /// <summary>
        /// Extracts Author Nickname from api response
        /// </summary>
        public string ExtractAuthorNickname(string apiResponse)
        {
            JsonElement author = GetAuthorElement(apiResponse);
            return author.GetProperty("nickname").GetString();
        }

        /// <summary>
        /// Extracts Author Verify Status from api response
        /// </summary>
        public bool ExtractAuthorVerify(string apiResponse)
        {
            JsonElement author = GetAuthorElement(apiResponse);
            string verify = author.GetProperty("custom_verify").GetString();
            return !string.IsNullOrEmpty(verify);
        }


        /// <summary>
        /// Extracts Author Region from api response
        /// </summary>
        public string ExtractAuthorRegion(string apiResponse)
        {
            JsonElement author = GetAuthorElement(apiResponse);
            string region = author.GetProperty("region").GetString();
            return region;
        }

        private string ExtractAuthorAvatarUrl(string apiResponse, string avatarProperty)
        {
            JsonElement author = GetAuthorElement(apiResponse);
            JsonElement urlList = author.GetProperty(avatarProperty).GetProperty("url_list");
            return urlList[urlList.GetArrayLength() - 1].GetString();
        }

        /// <summary>
        /// Extracts Author ThumbAvatarUrl from api response
        /// </summary>
        public string ExtractAuthorThumbAvatarUrl(string apiResponse) =>
            ExtractAuthorAvatarUrl(apiResponse, "avatar_thumb");

        /// <summary>
        /// Extracts Author MediumAvatarUrl from api response
        /// </summary>
        public string ExtractAuthorMediumAvatarUrl(string apiResponse) =>
            ExtractAuthorAvatarUrl(apiResponse, "avatar_medium");
    }
}