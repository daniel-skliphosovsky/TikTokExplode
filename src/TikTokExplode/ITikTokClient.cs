using TikTokExplode.Domain.Entities;

namespace TikTokExplode;

/// <summary>Provides access to TikTok metadata and download functionality.</summary>
public interface ITikTokClient
{
    /// <summary>Gets full publication metadata for a TikTok URL.</summary>
    Task<Publication> GetPublicationAsync(string url, CancellationToken ct = default);

    /// <summary>Gets video metadata for a TikTok URL.</summary>
    Task<Video> GetVideoAsync(string url, CancellationToken ct = default);

    /// <summary>Gets images metadata for a TikTok URL.</summary>
    Task<IReadOnlyList<Image>> GetImagesAsync(string url, CancellationToken ct = default);

    /// <summary>Downloads a video from a direct video URL to a file.</summary>
    Task DownloadVideoAsync(string videoUrl, string destinationFilePath, IProgress<long>? progress = null, CancellationToken ct = default);

    /// <summary>Downloads a single image from a direct image URL to a file.</summary>
    Task DownloadImageAsync(string imageUrl, string destinationFilePath, IProgress<long>? progress = null, CancellationToken ct = default);

    /// <summary>Downloads multiple images from a publication to a directory.</summary>
    Task DownloadImagesAsync(IReadOnlyList<Image> images, string destinationDirectory, IProgress<long>? progress = null, CancellationToken ct = default);
}
