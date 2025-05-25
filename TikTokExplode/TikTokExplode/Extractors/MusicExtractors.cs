using System.Text.Json;
using TikTokExplode.Exceptions;

namespace TikTokExplode.Extractors
{
	public partial class ApiExtractor
	{
        public string ExtractMusicId(string apiResponse)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(apiResponse);
                string id = doc.RootElement
                                .GetProperty("aweme_list")[0]
                                .GetProperty("music")
                                .GetProperty("id").GetString();

                return id;
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException($"Error parsing JSON response: {ex.Message}");
            }
        }

        public string ExtractMusicTitle(string apiResponse)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(apiResponse);
                string title = doc.RootElement
                                .GetProperty("aweme_list")[0]
                                .GetProperty("music")
                                .GetProperty("title").GetString();

                return title;
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException($"Error parsing JSON response: {ex.Message}");
            }
        }

        public string ExtractMusicAuthor(string apiResponse)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(apiResponse);
                string author = doc.RootElement
                                .GetProperty("aweme_list")[0]
                                .GetProperty("music")
                                .GetProperty("author").GetString();

                return author;
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException($"Error parsing JSON response: {ex.Message}");
            }
        }

        public string ExtractMusicLargeCover(string apiResponse)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(apiResponse);
                string cover = doc.RootElement
                                .GetProperty("aweme_list")[0]
                                .GetProperty("music")
                                .GetProperty("cover_large")
                                .GetProperty("url_list")[2].GetString();

                return cover;
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException($"Error parsing JSON response: {ex.Message}");
            }
        }

        public string ExtractMusicMediumCover(string apiResponse)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(apiResponse);
                string cover = doc.RootElement
                                .GetProperty("aweme_list")[0]
                                .GetProperty("music")
                                .GetProperty("cover_medium")
                                .GetProperty("url_list")[2].GetString();

                return cover;
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException($"Error parsing JSON response: {ex.Message}");
            }
        }

        public string ExtractMusicThumbCover(string apiResponse)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(apiResponse);
                string cover = doc.RootElement
                                .GetProperty("aweme_list")[0]
                                .GetProperty("music")
                                .GetProperty("cover_thumb")
                                .GetProperty("url_list")[2].GetString();

                return cover;
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException($"Error parsing JSON response: {ex.Message}");
            }
        }

        public string ExtractMusicSoundUrl(string apiResponse)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(apiResponse);
                string url = doc.RootElement
                                .GetProperty("aweme_list")[0]
                                .GetProperty("music")
                                .GetProperty("play_url")
                                .GetProperty("uri").GetString();

                return url;
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException($"Error parsing JSON response: {ex.Message}");
            }
        }
    }
}

