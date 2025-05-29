using TikTokExplode.Exceptions;
using TikTokExplode.Extractors;

namespace TikTokExplode.WebRequester
{
	public partial class WebRequestsHandler
    {

        /// <summary>
        /// Gets API response
        /// </summary>
        public async Task<string> GetApiResponseAsync(string fullUrl, int maxRetries = 3, CancellationToken cancellationToken = default)
        {
            int attempt = 0;
            while (attempt < maxRetries)
            {
                attempt++;

                CreateRandomHttpHeaders();

                ApiExtractor apiExtractor = new ApiExtractor();

                string awemeId = apiExtractor.ExtractPublicationId(fullUrl);

                if (string.IsNullOrEmpty(awemeId))
                    throw new TikTokExplodeException("Failed to extract aweme ID");

                HttpResponseMessage response = await _httpClient.GetAsync(_apiUrl.Replace("{awemeId}", awemeId), cancellationToken);
                string content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                    return content;
                else
                    return await GetApiResponseAsync(fullUrl);
            }
            throw new TikTokExplodeException($"API request failed after {maxRetries} attempts");
        }

        /// <summary>
        /// Get Author object by publication link
        /// </summary>
        public async Task<HttpResponseMessage> GetDownloadUrlResponse(string downloadUrl, CancellationToken cancellationToken = default)
        {
            CreateRandomHttpHeaders();

            HttpResponseMessage response = await _httpClient.GetAsync(downloadUrl ,cancellationToken);
            if (response.IsSuccessStatusCode)
                return response;
            else
                throw new TikTokExplodeException($"DownloadUrl request error: {(int)response.StatusCode} ({response.StatusCode})");
        }
    }
}

