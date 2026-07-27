using System.Text.RegularExpressions;

namespace TikTokExplode.Domain.Specifications;

public sealed partial class PublicationUrlSpecification : IPublicationUrlSpecification
{
    private static readonly Regex TikTokUrlRegex = GenerateTikTokUrlRegex();

    [GeneratedRegex(@"https?:\/\/(?:www\.)?(?:tiktok\.com|vm\.tiktok\.com)\/.+", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
    private static partial Regex GenerateTikTokUrlRegex();

    public bool IsSatisfiedBy(string? url)
    {
        return !string.IsNullOrWhiteSpace(url) && TikTokUrlRegex.IsMatch(url);
    }

    public string GetErrorMessage(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return "URL cannot be null or empty.";

        return "Invalid TikTok URL. URL must be from tiktok.com or vm.tiktok.com.";
    }
}
