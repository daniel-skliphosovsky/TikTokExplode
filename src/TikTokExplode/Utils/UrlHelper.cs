using System.Text.RegularExpressions;
using TikTokExplode.Exceptions;
using TikTokExplode.Publications;

namespace TikTokExplode;

/// <summary>
/// Validates TikTok URLs, resolves short links and extracts the aweme id.
/// </summary>
internal static partial class UrlHelper
{
    private static readonly HttpClient RedirectClient = CreateRedirectClient();

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

    public static async Task<string> ResolveAsync(string url, CancellationToken ct = default)
    {
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            using HttpResponseMessage response = await RedirectClient
                .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct)
                .ConfigureAwait(false);

            return response.RequestMessage?.RequestUri?.AbsoluteUri ?? url;
        }
        catch (Exception ex) when (ex is not OperationCanceledException || ex is TaskCanceledException { InnerException: TimeoutException })
        {
            // Non-TikTok or unreachable urls are returned as-is and validated later.
            return url;
        }
    }

    public static string? ExtractAwemeId(string fullUrl)
    {
        Match match = AwemeIdRegex().Match(fullUrl);
        return match.Success ? match.Groups[2].Value : null;
    }

    private static HttpClient CreateRedirectClient()
    {
        var client = new HttpClient(new HttpClientHandler { AllowAutoRedirect = true });
        client.Timeout = TimeSpan.FromSeconds(15);
        client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36");
        return client;
    }
}
