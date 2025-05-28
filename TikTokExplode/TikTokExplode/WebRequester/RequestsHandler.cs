namespace TikTokExplode.WebRequester
{
	public partial class WebRequestsHandler
    {
        private HttpClient _httpClient;

        public WebRequestsHandler()
        {
            _httpClient = new HttpClient();
            CreateRandomHttpHeaders();
        }

        //Thanks Xlinka (https://github.com/Xlinka) for public API app link 
        private static string _apiUrl { get; } = "https://api22-normal-c-alisg.tiktokv.com/aweme/v1/feed/?aweme_id={awemeId}&iid=7318518857994389254&device_id=7318517321748022790&channel=googleplay&app_name=musical_ly&version_code=300904&device_platform=android&device_type=ASUS_Z01QD&version=9";

        private void CreateRandomHttpHeaders()
        {
            string[] userAgents = new string[]
            {
                "Mozilla/5.0 (iPhone; CPU iPhone OS 15_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/15.0 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (Linux; Android 11; SM-G998B) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.120 Mobile Safari/537.36",
                "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.131 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36 Edg/91.0.864.59",
                "Mozilla/5.0 (iPad; CPU OS 14_6 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.0 Mobile/15E148 Safari/604.1"
            };

            
            string randomAgent = userAgents[new Random().Next(userAgents.Length)];
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(randomAgent);
        }
    }
}

