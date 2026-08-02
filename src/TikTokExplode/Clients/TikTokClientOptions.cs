namespace TikTokExplode;

/// <summary>
/// Configuration for <see cref="TikTokClient"/>.
/// </summary>
public class TikTokClientOptions
{
    // Public TikTok API endpoint. The {awemeId} placeholder is replaced with the
    // publication id extracted from the post url.
    public const string DefaultApiUrl =
        "https://api22-normal-c-alisg.tiktokv.com/aweme/v1/feed/?aweme_id={awemeId}" +
        "&iid=7318518857994389254&device_id=7318517321748022790&channel=googleplay" +
        "&app_name=musical_ly&version_code=300904&device_platform=android&device_type=ASUS_Z01QD&version=9";

    // Mirror hosts for the same feed endpoint. TikTok rate-limits individual
    // hosts aggressively (429), so the client tries them in order and falls
    // back to the next one when a host is rate-limited or returns non-JSON.
    private static readonly string[] ApiHosts =
    {
        "api22-normal-c-alisg.tiktokv.com",
        "api19-normal-c-alisg.tiktokv.com",
        "api16-normal-c-alisg.tiktokv.com"
    };

    /// <summary>
    /// TikTok API endpoint url. Must contain the {awemeId} placeholder.
    /// </summary>
    public string ApiUrl { get; set; } = DefaultApiUrl;

    /// <summary>
    /// Alternate hosts for the same API path, tried in order when the primary
    /// host rate-limits or returns a non-JSON response. When <see cref="ApiUrl"/>
    /// is left at the default value the placeholder path is rebuilt per host.
    /// </summary>
    public string[] MirrorHosts { get; set; } = ApiHosts;

    /// <summary>
    /// User agents used for requests. A random one is picked per request.
    /// Restored from the known-good v1.3.1 client.
    /// </summary>
    public string[] UserAgents { get; set; } =
    {
        "Mozilla/5.0 (iPhone; CPU iPhone OS 15_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/15.0 Mobile/15E148 Safari/604.1",
        "Mozilla/5.0 (Linux; Android 11; SM-G998B) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.120 Mobile Safari/537.36",
        "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.131 Safari/537.36",
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36 Edg/91.0.864.59",
        "Mozilla/5.0 (iPad; CPU OS 14_6 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.0 Mobile/15E148 Safari/604.1"
    };

    /// <summary>
    /// HTTP request timeout in seconds. Kept high (100s) to match the default
    /// HttpClient timeout of the known-good v1.3.1 client; TikTok's API can be
    /// slow on the first hit of a cold host.
    /// </summary>
    public int TimeoutSeconds { get; set; } = 100;

    /// <summary>
    /// Number of retries for transient failures (rate limits, 5xx, network errors).
    /// </summary>
    public int RetryCount { get; set; } = 3;

    /// <summary>
    /// Base delay in milliseconds for the exponential backoff between retries.
    /// </summary>
    public int RetryBaseDelayMs { get; set; } = 1000;

    /// <summary>
    /// Minimum interval in milliseconds between TikTok API calls.
    /// Helps avoid triggering rate limiting on bursty callers.
    /// Kept soft (1000ms) so a single slow request does not compound delays.
    /// </summary>
    public int MinRequestIntervalMs { get; set; } = 1000;
}
