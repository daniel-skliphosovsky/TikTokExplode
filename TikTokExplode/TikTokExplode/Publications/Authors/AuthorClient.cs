using TikTokExplode.Exceptions;
using TikTokExplode.Extractors;
using TikTokExplode.WebRequester;

namespace TikTokExplode.Publications.Authors
{
    public class AuthorClient
    {
        private readonly WebRequestsHandler _webRequestsHandler;
        private readonly ApiExtractor _apiExtractor;

        public AuthorClient()
        {
            _webRequestsHandler = new WebRequestsHandler();
            _apiExtractor = new ApiExtractor();
        }

        public async Task<Author> GetAsync(string publicationUrl)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(publicationUrl))
                    throw new TikTokExplodeException("Publication URL cannot be null or empty");

                string fullUrl = await _webRequestsHandler.GetFullUrl(publicationUrl);

                if (fullUrl == null)
                    throw new TikTokExplodeException("WebRequester returned null or empty url");


                if (!await _webRequestsHandler.IsUrlValid(fullUrl, PublicationClient.PublicationType.NoMetter))
                    throw new TikTokExplodeException("Invalid URL");

                string apiResponse = await _webRequestsHandler.GetApiResponse(fullUrl);

                return new Author
                {
                    UserId = _apiExtractor.ExtractAuthorUserId(apiResponse),
                    Nickname = _apiExtractor.ExtractAuthorNickname(apiResponse),
                    IsVerified = _apiExtractor.ExtractAuthorVerify(apiResponse),
                    ThumbAvatarUrl = _apiExtractor.ExtractAuthorThumbAvatarUrl(apiResponse),
                    MediumAvatarUrl = _apiExtractor.ExtractAuthorMediumAvatarUrl(apiResponse)
                };
            }
            catch (TikTokExplodeException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException($"Error retrieving author: {ex.Message}");
            }
        }
    }
}