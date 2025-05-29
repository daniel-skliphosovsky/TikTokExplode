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

        /// <summary>
        /// Gets Statistics object by publication link
        /// </summary>
        public async Task<Stats> GetAsync(string publicationUrl, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!await _webRequestsHandler.IsUrlValidAsync(publicationUrl, PublicationClient.PublicationType.NoMetter))
                    throw new TikTokExplodeException("Invalid URL");

                string apiResponse = await _webRequestsHandler.GetApiResponseAsync(publicationUrl, cancellationToken: cancellationToken);

                return new Stats
                {
                    CommentCount = _apiExtractor.ExtractCommentCount(apiResponse),
                    ShareCount = _apiExtractor.ExtractShareCount(apiResponse),
                    DownloadCount = _apiExtractor.ExtractDownloadCount(apiResponse),
                    PlayCount = _apiExtractor.ExtractPlayCount(apiResponse),
                    DiggCount = _apiExtractor.ExtractDiggCount(apiResponse),
                    ForwardCount = _apiExtractor.ExtractForwardCount(apiResponse),
                    RepostCount = _apiExtractor.ExtractRepostCount(apiResponse)
                };
            }
            catch (TikTokExplodeException)
            {
                throw;
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                throw new TikTokExplodeException($"Error retrieving statistics: {ex.Message}");
            }
        }
    }
}

