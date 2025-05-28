using TikTokExplode.Publications.Videos;
using TikTokExplode.Publications.Authors;
using TikTokExplode.Publications.Soundtracks;
using TikTokExplode.Publications.Images;
using TikTokExplode.Publications.Statistics;

namespace TikTokExplode.Publications
{
	public class Publication
	{
		public Author Author { get; set; }

		public Video? Video { get; set; }

		public List<Image>? Images { get; set; }

		public Soundtrack Soundtrack { get; set; }

		public Stats Statistics { get; set; }

		public bool IsAds { get; set; }

		public string Description { get; set; }

		public string Id { get; set; }
	}
}

