using System.Text.Json;
using TikTokExplode.Exceptions;

namespace TikTokExplode.Extractors
{
	public partial class ApiExtractor
	{
        public ulong ExtractAuthorUserId(string apiResponse)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(apiResponse);
                ulong userId = doc.RootElement
                                .GetProperty("aweme_list")[0]
                                .GetProperty("author")
                                .GetProperty("uid").GetUInt64();

                return userId;
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException($"Error parsing JSON response: {ex.Message}");
            }
        }

        public string ExtractAuthorNickname(string apiResponse)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(apiResponse);
                string nickname = doc.RootElement
                                .GetProperty("aweme_list")[0]
                                .GetProperty("author")
                                .GetProperty("nickname").GetString();

                return nickname;
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException($"Error parsing JSON response: {ex.Message}");
            }
        }

        public bool ExtractAuthorVerify(string apiResponse)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(apiResponse);
                string verify = doc.RootElement
                                .GetProperty("aweme_list")[0]
                                .GetProperty("author")
                                .GetProperty("custom_verify").GetString();

                return string.IsNullOrEmpty(verify) ? false : true;
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException($"Error parsing JSON response: {ex.Message}");
            }
        }

        public string ExtractAuthorThumbAvatarUrl(string apiResponse)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(apiResponse);
                string avatarUrl = doc.RootElement
                                .GetProperty("aweme_list")[0]
                                .GetProperty("author")
                                .GetProperty("avatar_thumb")
                                .GetProperty("url_list")[2].GetString();

                return avatarUrl;
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException($"Error parsing JSON response: {ex.Message}");
            }
        }

        public string ExtractAuthorMediumAvatarUrl(string apiResponse)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(apiResponse);
                string avatarUrl = doc.RootElement
                                .GetProperty("aweme_list")[0]
                                .GetProperty("author")
                                .GetProperty("avatar_medium")
                                .GetProperty("url_list")[2].GetString();

                return avatarUrl;
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException($"Error parsing JSON response: {ex.Message}");
            }
        }
    }
}

