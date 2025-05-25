namespace TikTokExplode.WebRequester
{
	public partial class WebRequestsHandler
    {
		public async Task<string> GetApiResponse(string videoId)
		{
            HttpResponseMessage response =  await _httpClient.GetAsync(_apiUrl.Replace("{videoId}", videoId));
            string content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return content;
            else
                return null; 
        } 
	}
}

