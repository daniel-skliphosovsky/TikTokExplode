using TikTokExplode.WebRequester;
using TikTokExplode.Publications;

namespace TikTokExplode
{
    public class TikTokClient
    {
        public PublicationClient Publications { get; }

        public TikTokClient()
        {
            Publications = new PublicationClient();
        }
    }
}