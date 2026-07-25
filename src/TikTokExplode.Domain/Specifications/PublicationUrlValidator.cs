using System.Text.RegularExpressions;
using TikTokExplode.Domain.ValueObjects;

namespace TikTokExplode.Domain.Specifications;

public static partial class PublicationUrlValidator
{
    private static readonly Regex TikTokUrlRegex = GenerateTikTokUrlRegex();

    [GeneratedRegex(@"https?:\/\/(?:www\.)?(?:tiktok\.com|vm\.tiktok\.com)\/.+", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
    private static partial Regex GenerateTikTokUrlRegex();

    /// <summary>
    /// Validates whether a given URL is a valid TikTok URL.
    /// </summary>
    /// <param name="url">The URL to validate.</param>
    /// <returns><c>true</c> if the URL is a valid TikTok URL; otherwise <c>false</c>.</returns>
    public static bool IsValid(string? url)
    {
        return !string.IsNullOrWhiteSpace(url) && TikTokUrlRegex.IsMatch(url);
    }
}
