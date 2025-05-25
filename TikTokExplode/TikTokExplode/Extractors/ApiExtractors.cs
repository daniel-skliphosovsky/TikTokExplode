using System.Text.RegularExpressions;

namespace TikTokExplode.Extractors
{
	public partial class ApiExtractor
	{
	    public string ExtractPublicationId(string url)
        {
            var match = Regex.Match(url, @"https:\/\/www\.tiktok\.com\/@[^/]+\/(video|photo)\/(\d+)");
            return match.Success ? match.Groups[1].Value : null;
        }
    }
}

