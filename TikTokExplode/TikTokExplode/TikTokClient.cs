using TikTokExplode.Publications;

namespace TikTokExplode
{
    public class TikTokClient
    {
        public PublicationClient Publications { get; }

        private HttpClient _httpClient;

        public TikTokClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public TikTokClient()
        {
            _httpClient = CreateRandomHttpClient();
        }

        private HttpClient CreateRandomHttpClient()
        {
            HttpClient httpClient = new HttpClient();

            string[] userAgents = new string[]
            {
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.0.3 Safari/605.1.15",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:87.0) Gecko/20100101 Firefox/87.0",
                "Mozilla/5.0 (iPhone; CPU iPhone OS 15_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/15.0 Mobile/15E148 Safari/604.1",
                "Mozilla/5.0 (Linux; Android 11; SM-G998B) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.120 Mobile Safari/537.36",
                "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.131 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36 Edg/91.0.864.59",
                "Mozilla/5.0 (iPad; CPU OS 14_6 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.0 Mobile/15E148 Safari/604.1"
            };

            string randomAgent = userAgents[new Random().Next(userAgents.Length)];
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(randomAgent);

            return httpClient;
        }
    }
}