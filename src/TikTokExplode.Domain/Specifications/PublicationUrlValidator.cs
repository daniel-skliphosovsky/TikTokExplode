using System.Text.RegularExpressions;
using TikTokExplode.Domain.ValueObjects;

namespace TikTokExplode.Domain.Specifications;

public static partial class PublicationUrlValidator
{
    private static readonly Regex TikTokUrlRegex = GenerateTikTokUrlRegex();

    [GeneratedRegex(@"https?:\/\/(?:www\.)?(?:tiktok\.com|vm\.tiktok\.com)\/.+", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
    private static partial Regex GenerateTikTokUrlRegex();

    public static bool IsValid(string url)
    {
        return !string.IsNullOrWhiteSpace(url) && TikTokUrlRegex.IsMatch(url);
    }
}
