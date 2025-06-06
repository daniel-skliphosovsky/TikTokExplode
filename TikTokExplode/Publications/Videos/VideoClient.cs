﻿using System.Threading.Tasks;
using TikTokExplode.Exceptions;
using TikTokExplode.Extractors;
using TikTokExplode.WebRequester;

namespace TikTokExplode.Publications.Videos
{
    public partial class VideoClient
    {
        private readonly WebRequestsHandler _webRequestsHandler;
        private readonly ApiExtractor _apiExtractor;

        public VideoClient()
        {
            _webRequestsHandler = new WebRequestsHandler();
            _apiExtractor = new ApiExtractor();
        }

        /// <summary>
        /// Gets Video object by publication link
        /// </summary>
        public async Task<Video> GetAsync(string publicationUrl, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!await _webRequestsHandler.IsUrlValidAsync(publicationUrl, PublicationClient.PublicationType.Video))
                    throw new TikTokExplodeException("Invalid URL");

                string apiResponse = await _webRequestsHandler.GetApiResponseAsync(publicationUrl, cancellationToken: cancellationToken);

                return new Video
                {
                    AwemeId = _apiExtractor.ExtractPublicationId(await _webRequestsHandler.GetFullUrlAsync(publicationUrl)),
                    Url = _apiExtractor.ExtractVideoUrl(apiResponse),
                    Width = _apiExtractor.ExtractVideoWidth(apiResponse),
                    Height = _apiExtractor.ExtractVideoHeight(apiResponse),
                    Duration = _apiExtractor.ExtractVideoDuration(apiResponse)
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
                throw new TikTokExplodeException($"Error retrieving video: {ex.Message}");
            }
        }
    }
}