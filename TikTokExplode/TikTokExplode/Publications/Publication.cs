using TikTokExplode.Publications.Video;
using TikTokExplode.Publications.Author;
using TikTokExplode.Publications.Music;
using TikTokExplode.Publications.Image;

namespace TikTokExplode.Publication
{
	public class Publication
	{
		public Author Author { get; set; }

		public Video Video { get; set; }

		public List<Image> Images { get; set; }

		public Music Music { get; set; }
	}
}

