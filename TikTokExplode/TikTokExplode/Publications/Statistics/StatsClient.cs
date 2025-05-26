using TikTokExplode.Exceptions;
using TikTokExplode.Extractors;
using TikTokExplode.WebRequester;

namespace TikTokExplode.Publications.Statistics
{
    public class StatsClient
    {
        private readonly WebRequestsHandler _webRequestsHandler;
        private readonly ApiExtractor _apiExtractor;

        public StatsClient()
        {
            _webRequestsHandler = new WebRequestsHandler();
            _apiExtractor = new ApiExtractor();
        }

        public async Task<Stats> GetAsync(string publicationUrl)
        {
            try
            {
                string fullUrl = await _webRequestsHandler.GetFullUrl(publicationUrl);

                if (!(await _webRequestsHandler.IsUrlValid(fullUrl) && (fullUrl.Contains("/video/") || fullUrl.Contains("/photo/"))))
                    throw new TikTokExplodeException("Invalid URL");

                string apiResponse = await _webRequestsHandler.GetApiResponse(fullUrl);

                return new Stats
                {
                    CommentCount = _apiExtractor.ExtractCommentCount(apiResponse),
                    ShareCount = _apiExtractor.ExtractShareCount(apiResponse),
                    DownloadingCount = _apiExtractor.ExtractDownloadCount(apiResponse),
                    PlayCount = _apiExtractor.ExtractPlayCount(apiResponse)
                };
            }
            catch (TikTokExplodeException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException($"Error retrieving statistics: {ex.Message}");
            }
        }
    }
}

