using System.Text.Json.Serialization;

namespace TikTokExplode;

[JsonSerializable(typeof(TikTokApiResponse))]
[JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true)]
internal partial class TikTokApiJsonContext : JsonSerializerContext
{
}

internal class TikTokApiResponse
{
    [JsonPropertyName("aweme_list")]
    public List<AwemeDto>? AwemeList { get; set; }
}

internal class AwemeDto
{
    [JsonPropertyName("aweme_id")]
    public string AwemeId { get; set; } = string.Empty;

    [JsonPropertyName("desc")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("is_ads")]
    public bool IsAds { get; set; }

    [JsonPropertyName("author")]
    public AuthorDto? Author { get; set; }

    [JsonPropertyName("video")]
    public VideoDto? Video { get; set; }

    [JsonPropertyName("image_post_info")]
    public ImagePostInfoDto? ImagePostInfo { get; set; }

    [JsonPropertyName("music")]
    public MusicDto? Music { get; set; }

    [JsonPropertyName("statistics")]
    public StatisticsDto? Statistics { get; set; }
}

internal class AuthorDto
{
    [JsonPropertyName("uid")]
    public string Uid { get; set; } = string.Empty;

    [JsonPropertyName("nickname")]
    public string Nickname { get; set; } = string.Empty;

    [JsonPropertyName("verified")]
    public bool Verified { get; set; }

    [JsonPropertyName("custom_verify")]
    public string CustomVerify { get; set; } = string.Empty;

    [JsonPropertyName("avatar_thumb")]
    public UrlListDto? AvatarThumb { get; set; }

    [JsonPropertyName("avatar_medium")]
    public UrlListDto? AvatarMedium { get; set; }

    [JsonPropertyName("region")]
    public string Region { get; set; } = string.Empty;
}

internal class VideoDto
{
    [JsonPropertyName("play_addr")]
    public UrlListDto? PlayAddr { get; set; }

    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("duration")]
    public int Duration { get; set; }
}

internal class ImagePostInfoDto
{
    [JsonPropertyName("images")]
    public List<ImageDto>? Images { get; set; }
}

internal class ImageDto
{
    [JsonPropertyName("display_image")]
    public DisplayImageDto? DisplayImage { get; set; }
}

internal class DisplayImageDto
{
    [JsonPropertyName("url_list")]
    public List<string>? UrlList { get; set; }

    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }
}

internal class MusicDto
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("author")]
    public string Author { get; set; } = string.Empty;

    [JsonPropertyName("play_url")]
    public UrlListDto? PlayUrl { get; set; }

    [JsonPropertyName("cover_large")]
    public UrlListDto? CoverLarge { get; set; }

    [JsonPropertyName("cover_medium")]
    public UrlListDto? CoverMedium { get; set; }

    [JsonPropertyName("cover_thumb")]
    public UrlListDto? CoverThumb { get; set; }
}

internal class StatisticsDto
{
    [JsonPropertyName("comment_count")]
    public long CommentCount { get; set; }

    [JsonPropertyName("digg_count")]
    public long DiggCount { get; set; }

    [JsonPropertyName("download_count")]
    public long DownloadCount { get; set; }

    [JsonPropertyName("play_count")]
    public long PlayCount { get; set; }

    [JsonPropertyName("share_count")]
    public long ShareCount { get; set; }

    [JsonPropertyName("forward_count")]
    public long ForwardCount { get; set; }

    [JsonPropertyName("repost_count")]
    public long RepostCount { get; set; }
}

internal class UrlListDto
{
    [JsonPropertyName("url_list")]
    public List<string>? UrlList { get; set; }
}
