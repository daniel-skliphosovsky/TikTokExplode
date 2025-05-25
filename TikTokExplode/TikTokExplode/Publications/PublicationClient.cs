using TikTokExplode.Publications.Videos;

namespace TikTokExplode.Publications
{
	public class PublicationClient
    {
        public VideoClient Videos { get; set; }

        public PublicationClient()
        {
            Videos = new VideoClient();
        }
    }
}

