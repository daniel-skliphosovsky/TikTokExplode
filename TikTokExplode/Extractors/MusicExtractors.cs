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

        /// <summary>
        /// Extracts Music Id from api response
        /// </summary>
        public ulong ExtractMusicId(string apiResponse)
        {
            JsonElement music = GetMusicElement(apiResponse);
            return music.GetProperty("id").GetUInt64();
        }

        /// <summary>
        /// Extracts Music Title from api response
        /// </summary>
        public string ExtractMusicTitle(string apiResponse)
        {
            JsonElement music = GetMusicElement(apiResponse);
            return music.GetProperty("title").GetString();
        }

        /// <summary>
        /// Extracts Music Author from api response
        /// </summary>
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

        /// <summary>
        /// Extracts Music LargeCoverUrl from api response
        /// </summary>
        public string ExtractMusicLargeCover(string apiResponse) =>
            ExtractCoverUrl(apiResponse, "cover_large");

        /// <summary>
        /// Extracts Music MediumCoverUrl from api response
        /// </summary>
        public string ExtractMusicMediumCover(string apiResponse) =>
            ExtractCoverUrl(apiResponse, "cover_medium");

        /// <summary>
        /// Extracts Music ThumbCoverUrl from api response
        /// </summary>
        public string ExtractMusicThumbCover(string apiResponse) =>
            ExtractCoverUrl(apiResponse, "cover_thumb");

        /// <summary>
        /// Extracts Music SoundUrl from api response
        /// </summary>
        public string ExtractMusicSoundUrl(string apiResponse)
        {
            JsonElement music = GetMusicElement(apiResponse);
            return music.GetProperty("play_url").GetProperty("uri").GetString();
        }
    }
}