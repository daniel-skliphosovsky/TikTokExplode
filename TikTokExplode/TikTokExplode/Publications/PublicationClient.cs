using TikTokExplode.Publications.Videos;
using TikTokExplode.Publications.Images;
using TikTokExplode.Publications.Authors;
using TikTokExplode.Publications.Musics;
using TikTokExplode.Publications.Statistics;

namespace TikTokExplode.Publications
{
	public class PublicationClient
    {
        public VideoClient Videos { get; }

        public AuthorClient Authors { get; }

        public ImageClient Images { get; }

        public MusicClient Musics { get; }

        public StatsClient Statistics { get; }


        public PublicationClient()
        {
            Videos = new VideoClient();
            Authors = new AuthorClient();
            Images = new ImageClient();
            Musics = new MusicClient();
            Statistics = new StatsClient();
        }
    }
}

