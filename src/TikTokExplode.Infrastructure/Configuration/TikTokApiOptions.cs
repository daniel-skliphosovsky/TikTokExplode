using System.ComponentModel.DataAnnotations;

namespace TikTokExplode.Infrastructure.Configuration;

public sealed class TikTokApiOptions
{
    public const string SectionName = "TikTokApi";

    public string BaseUrl { get; set; } = "https://www.tiktok.com";
    public string ApiUrl { get; set; } = "https://api22-normal-c-alisg.tiktokv.com";
    public string[] UserAgents { get; set; } = new[]
    {
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36",
        "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36",
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36",
        "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36"
    };
    public int TimeoutSeconds { get; set; } = 30;
    public int MaxRetries { get; set; } = 3;
    public int RetryDelayMilliseconds { get; set; } = 1000;
}
