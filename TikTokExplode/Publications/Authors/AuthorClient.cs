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

        /// <summary>
        /// Gets Author object by publication link
        /// </summary>
        public async Task<Author> GetAsync(string publicationUrl, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!await _webRequestsHandler.IsUrlValidAsync(publicationUrl, PublicationClient.PublicationType.NoMetter))
                    throw new TikTokExplodeException("Invalid URL");

                string apiResponse = await _webRequestsHandler.GetApiResponseAsync(publicationUrl, cancellationToken: cancellationToken);

                return new Author
                {
                    UserId = _apiExtractor.ExtractAuthorUserId(apiResponse),
                    Nickname = _apiExtractor.ExtractAuthorNickname(apiResponse),
                    IsVerified = _apiExtractor.ExtractAuthorVerify(apiResponse),
                    ThumbAvatarUrl = _apiExtractor.ExtractAuthorThumbAvatarUrl(apiResponse),
                    MediumAvatarUrl = _apiExtractor.ExtractAuthorMediumAvatarUrl(apiResponse),
                    Region = _apiExtractor.ExtractAuthorRegion(apiResponse)
                };
            }
            catch (TikTokExplodeException)
            {
                throw;
            }
            catch (OperationCanceledException)
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