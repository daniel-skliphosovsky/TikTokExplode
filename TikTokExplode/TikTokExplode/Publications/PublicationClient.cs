using TikTokExplode.Publications.Videos;
using TikTokExplode.Publications.Images;
using TikTokExplode.Publications.Authors;

namespace TikTokExplode.Publications
{
	public class PublicationClient
    {
        public VideoClient Videos { get; set; }

        public AuthorClient Authors { get; set; }

        public ImageClient Images { get; set; }


        public PublicationClient()
        {
            Videos = new VideoClient();
            Authors = new AuthorClient();
            Images = new ImageClient();
        }
    }
}

