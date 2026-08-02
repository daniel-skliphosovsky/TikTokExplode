using System.Net;
using System.Text.RegularExpressions;
using TikTokExplode.Exceptions;
using TikTokExplode.Publications;

namespace TikTokExplode;

/// <summary>
/// Validates TikTok URLs, resolves short links and extracts the aweme id.
/// </summary>
internal static partial class UrlHelper
{
    [GeneratedRegex(@"https?:\/\/(?:www\.)?(?:tiktok\.com|vm\.tiktok\.com|vt\.tiktok\.com)\/.+", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
    private static partial Regex TikTokUrlRegex();

    [GeneratedRegex(@"https:\/\/www\.tiktok\.com\/@[^/]+\/(video|photo)\/(\d+)", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
    private static partial Regex AwemeIdRegex();

    public static void Validate(string url)
    {
        if (string.IsNullOrWhiteSpace(url) || !TikTokUrlRegex().IsMatch(url))
            throw new ValidationException("Invalid URL");
    }

    /// <summary>
    /// Resolves the url (follows short-link redirects) and checks that it points
    /// to a TikTok publication of the expected type.
    /// </summary>
    public static async Task<string> ResolveAndValidateAsync(string url, PublicationClient.PublicationType type, CancellationToken ct = default)
    {
        Validate(url);

        string fullUrl = await ResolveAsync(url, ct).ConfigureAwait(false);
        PublicationClient.PublicationType actualType = PublicationClient.GetPublicationTypeFromUrl(fullUrl);

        bool isValid = type switch
        {
            PublicationClient.PublicationType.Video => actualType == PublicationClient.PublicationType.Video,
            PublicationClient.PublicationType.Images => actualType == PublicationClient.PublicationType.Images,
            _ => actualType is PublicationClient.PublicationType.Video or PublicationClient.PublicationType.Images
        };

        if (!isValid)
            throw new ValidationException("Invalid URL");

        return fullUrl;
    }

    /// <summary>
    /// Resolves a (possibly short) TikTok url to its final form by following
    /// redirects. Restored from the known-good v1.3.1 mechanism: HttpWebRequest
    /// with auto-redirect, returning the final ResponseUri. Retried a few times
    /// because short-link resolution is slow and occasionally times out.
    /// </summary>
    public static async Task<string> ResolveAsync(string url, CancellationToken ct = default)
    {
        for (int attempt = 0; attempt < 3; attempt++)
        {
            try
            {
#pragma warning disable SYSLIB0014 // v1.3.1 short-link resolution mechanism
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36";
                request.Timeout = 100_000;

                using WebResponse response = await request.GetResponseAsync().WaitAsync(ct).ConfigureAwait(false);
#pragma warning restore SYSLIB0014
                return response.ResponseUri?.AbsoluteUri ?? url;
            }
            catch (Exception ex) when (ex is not OperationCanceledException || ex is TaskCanceledException { InnerException: TimeoutException })
            {
                // Transient short-link failures: try again, then return the
                // url as-is (validated by the caller) if all attempts fail.
            }
        }

        return url;
    }

    public static string? ExtractAwemeId(string fullUrl)
    {
        Match match = AwemeIdRegex().Match(fullUrl);
        return match.Success ? match.Groups[2].Value : null;
    }
}
