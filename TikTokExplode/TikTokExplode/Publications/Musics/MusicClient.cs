using TikTokExplode.Exceptions;
using TikTokExplode.Extractors;
using TikTokExplode.WebRequester;

namespace TikTokExplode.Publications.Musics
{
	public class MusicClient
	{
        public Music GetAsync(string publicationUrl)
        {
            string fullUrl = WebRequestsHandler.GetFullUrl(publicationUrl).Result;

            if (!(WebRequestsHandler.IsUrlValid(fullUrl).Result && (fullUrl.Contains("/video/") || fullUrl.Contains("/photo/"))))
                throw new TikTokExplodeException("Invalid URL");

            string apiResponse = new WebRequestsHandler().GetApiResponse(fullUrl).Result;

            ApiExtractor apiExtractor = new ApiExtractor();
            Music music = new Music()
            {
                Author = apiExtractor.ExtractMusicAuthor(apiResponse),
                Title = apiExtractor.ExtractMusicTitle(apiResponse),
                Id = apiExtractor.ExtractMusicId(apiResponse),
                LargeCoverUrl = apiExtractor.ExtractMusicLargeCover(apiResponse),
                MediumCoverUrl = apiExtractor.ExtractAuthorMediumAvatarUrl(apiResponse),
                ThumbCoverUrl = apiExtractor.ExtractMusicThumbCover(apiResponse),
                SoundUrl = apiExtractor.ExtractMusicSoundUrl(apiResponse)
            };

            return music;
        }
    }
}

