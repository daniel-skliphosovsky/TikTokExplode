using TikTokExplode.Exceptions;
using TikTokExplode.Extractors;
using TikTokExplode.WebRequester;

namespace TikTokExplode.Publications.Authors
{
	public class AuthorClient
	{
        public Author GetAsync(string publicationUrl)
        {
            string fullUrl = WebRequestsHandler.GetFullUrl(publicationUrl).Result;

            if (!(WebRequestsHandler.IsUrlValid(fullUrl).Result && (fullUrl.Contains("/video/") || fullUrl.Contains("/photo/"))))
                throw new TikTokExplodeException("Invalid URL");

            string apiResponse = new WebRequestsHandler().GetApiResponse(fullUrl).Result;

            ApiExtractor apiExtractor = new ApiExtractor();
            Author author = new Author()
            {
                UserId = apiExtractor.ExtractAuthorUserId(apiResponse),
                Nickname = apiExtractor.ExtractAuthorNickname(apiResponse),
                IsVerified = apiExtractor.ExtractAuthorVerify(apiResponse),
                ThumbAvatarUrl = apiExtractor.ExtractAuthorThumbAvatarUrl(apiResponse),
                MediumAvatarUrl = apiExtractor.ExtractAuthorMediumAvatarUrl(apiResponse)
            };

            return author;
        }
    }
}

