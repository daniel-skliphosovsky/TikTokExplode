using TikTokExplode.Exceptions;
using TikTokExplode.Extractors;
using TikTokExplode.WebRequester;

namespace TikTokExplode.Publications.Soundtracks
{
    public class SoundtrackClient
    {
        private readonly WebRequestsHandler _webRequestsHandler;
        private readonly ApiExtractor _apiExtractor;

        public SoundtrackClient()
        {
            _webRequestsHandler = new WebRequestsHandler();
            _apiExtractor = new ApiExtractor();
        }

        /// <summary>
        /// Gets Music object by publication link
        /// </summary>
        public async Task<Soundtrack> GetAsync(string publicationUrl, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!await _webRequestsHandler.IsUrlValidAsync(publicationUrl, PublicationClient.PublicationType.NoMetter))
                    throw new TikTokExplodeException("Invalid URL");

                string apiResponse = await _webRequestsHandler.GetApiResponseAsync(publicationUrl, cancellationToken: cancellationToken);

                return new Soundtrack
                {
                    Id = _apiExtractor.ExtractMusicId(apiResponse),
                    Title = _apiExtractor.ExtractMusicTitle(apiResponse),
                    Author = _apiExtractor.ExtractMusicAuthor(apiResponse),
                    SoundUrl = _apiExtractor.ExtractMusicSoundUrl(apiResponse),
                    LargeCoverUrl = _apiExtractor.ExtractMusicLargeCover(apiResponse),
                    MediumCoverUrl = _apiExtractor.ExtractMusicMediumCover(apiResponse),
                    ThumbCoverUrl = _apiExtractor.ExtractMusicThumbCover(apiResponse)
                };
            }
            catch (TikTokExplodeException)
            {
                throw;
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                throw new TikTokExplodeException($"Error retrieving music: {ex.Message}");
            }
        }
    }
}