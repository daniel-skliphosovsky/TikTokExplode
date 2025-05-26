using TikTokExplode.Publications.Videos;
using TikTokExplode.Publications.Authors;
using TikTokExplode.Publications.Musics;
using TikTokExplode.Publications.Images;
using TikTokExplode.Publications.Statistics;

namespace TikTokExplode.Publications
{
	public class Publication
	{
		public Author Author { get; set; }

		public Video? Video { get; set; }

		public List<Image>? Images { get; set; }

		public Music Music { get; set; }

		public Stats Statistics { get; set; }

		public bool IsAds { get; set; }

		public string Description { get; set; }

		public string Id { get; set; }
	}
}

