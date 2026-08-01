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

    /// <summary>
    /// TikTok API endpoint url. Must contain the {awemeId} placeholder.
    /// </summary>
    public string ApiUrl { get; set; } = DefaultApiUrl;

    /// <summary>
    /// User agents used for requests. A random one is picked per request.
    /// </summary>
    public string[] UserAgents { get; set; } =
    {
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36",
        "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36",
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36",
        "Mozilla/5.0 (Linux; Android 11; SM-G998B) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.120 Mobile Safari/537.36"
    };

    /// <summary>
    /// HTTP request timeout in seconds.
    /// </summary>
    public int TimeoutSeconds { get; set; } = 30;

    /// <summary>
    /// Number of retries for transient failures (rate limits, 5xx, network errors).
    /// </summary>
    public int RetryCount { get; set; } = 3;

    /// <summary>
    /// Base delay in milliseconds for the exponential backoff between retries.
    /// </summary>
    public int RetryBaseDelayMs { get; set; } = 500;
}
