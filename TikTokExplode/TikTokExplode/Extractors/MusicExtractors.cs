using System.Text.Json;
using TikTokExplode.Exceptions;

namespace TikTokExplode.Extractors
{
    public partial class ApiExtractor
    {
        private static JsonElement GetMusicElement(string apiResponse)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(apiResponse);
                return doc.RootElement
                    .GetProperty("aweme_list")[0]
                    .GetProperty("music").Clone();
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException($"Error parsing JSON response: {ex.Message}");
            }
        }

        public ulong ExtractMusicId(string apiResponse)
        {
            JsonElement music = GetMusicElement(apiResponse);
            return music.GetProperty("id").GetUInt64();
        }

        public string ExtractMusicTitle(string apiResponse)
        {
            JsonElement music = GetMusicElement(apiResponse);
            return music.GetProperty("title").GetString();
        }

        public string ExtractMusicAuthor(string apiResponse)
        {
            JsonElement music = GetMusicElement(apiResponse);
            return music.GetProperty("author").GetString();
        }

        private string ExtractCoverUrl(string apiResponse, string coverProperty)
        {
            JsonElement music = GetMusicElement(apiResponse);
            JsonElement urlList = music.GetProperty(coverProperty).GetProperty("url_list");
            return urlList[urlList.GetArrayLength() - 1].GetString();
        }

        public string ExtractMusicLargeCover(string apiResponse) =>
            ExtractCoverUrl(apiResponse, "cover_large");

        public string ExtractMusicMediumCover(string apiResponse) =>
            ExtractCoverUrl(apiResponse, "cover_medium");

        public string ExtractMusicThumbCover(string apiResponse) =>
            ExtractCoverUrl(apiResponse, "cover_thumb");

        public string ExtractMusicSoundUrl(string apiResponse)
        {
            JsonElement music = GetMusicElement(apiResponse);
            return music.GetProperty("play_url").GetProperty("uri").GetString();
        }
    }
}