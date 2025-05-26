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

        public string ExtractAuthorUserId(string apiResponse)
        {
            JsonElement author = GetAuthorElement(apiResponse);
            return author.GetProperty("uid").GetString();
        }

        public string ExtractAuthorNickname(string apiResponse)
        {
            JsonElement author = GetAuthorElement(apiResponse);
            return author.GetProperty("nickname").GetString();
        }

        public bool ExtractAuthorVerify(string apiResponse)
        {
            JsonElement author = GetAuthorElement(apiResponse);
            string verify = author.GetProperty("custom_verify").GetString();
            return !string.IsNullOrEmpty(verify);
        }

        private string ExtractAuthorAvatarUrl(string apiResponse, string avatarProperty)
        {
            JsonElement author = GetAuthorElement(apiResponse);
            JsonElement urlList = author.GetProperty(avatarProperty).GetProperty("url_list");
            return urlList[urlList.GetArrayLength() - 1].GetString();
        }

        public string ExtractAuthorThumbAvatarUrl(string apiResponse) =>
            ExtractAuthorAvatarUrl(apiResponse, "avatar_thumb");

        public string ExtractAuthorMediumAvatarUrl(string apiResponse) =>
            ExtractAuthorAvatarUrl(apiResponse, "avatar_medium");
    }
}