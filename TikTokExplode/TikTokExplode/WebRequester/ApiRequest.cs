using TikTokExplode.Exceptions;
using TikTokExplode.Extractors;

namespace TikTokExplode.WebRequester
{
	public partial class WebRequestsHandler
    {
		public async Task<string> GetApiResponse(string fullUrl)
		{
            ApiExtractor apiExtractor = new ApiExtractor();

            string awemeId = apiExtractor.ExtractPublicationId(fullUrl);

            if (string.IsNullOrEmpty(awemeId))
                throw new TikTokExplodeException("Failed to extract aweme ID");

            HttpResponseMessage response =  await _httpClient.GetAsync(_apiUrl.Replace("{awemeId}", awemeId));
            string content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return content;
            else
                throw new TikTokExplodeException("API request return failure Status Code");
        }            
    }
}

