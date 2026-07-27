using TikTokExplode.Domain.Entities;
using TikTokExplode.Domain.Exceptions;
using TikTokExplode.Domain.Interfaces;
using TikTokExplode.Domain.Specifications;


namespace TikTokExplode;

/// <summary>
/// Main facade for interacting with TikTok content.
/// Provides unified access to publication metadata and media downloading.
/// </summary>
public sealed class TikTokClient : ITikTokClient
{
    private readonly IPublicationRepository _publicationRepository;
    private readonly IVideoRepository _videoRepository;
    private readonly IImageRepository _imageRepository;
    private readonly IFileDownloader _fileDownloader;

    public TikTokClient(
        IPublicationRepository publicationRepository,
        IVideoRepository videoRepository,
        IImageRepository imageRepository,
        IFileDownloader fileDownloader)
    {
        _publicationRepository = publicationRepository;
        _videoRepository = videoRepository;
        _imageRepository = imageRepository;
        _fileDownloader = fileDownloader;
    }

    /// <summary>
    /// Gets full publication metadata (author, video/images, soundtrack, stats) for a TikTok URL.
    /// </summary>
    /// <param name="url">TikTok video or image post URL.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Complete publication with all related entities.</returns>
    /// <exception cref="ValidationException">Thrown when URL is not a valid TikTok URL.</exception>
    /// <exception cref="ApiException">Thrown when TikTok API returns an error.</exception>
    public async Task<Publication> GetPublicationAsync(string url, CancellationToken ct = default)
    {
        ValidateUrl(url);
        return await _publicationRepository.GetByUrlAsync(url, ct);
    }

    /// <summary>
    /// Gets video metadata for a TikTok URL.
    /// </summary>
    public async Task<Video> GetVideoAsync(string url, CancellationToken ct = default)
    {
        ValidateUrl(url);
        return await _videoRepository.GetByUrlAsync(url, ct);
    }

    /// <summary>
    /// Gets image metadata for a TikTok URL.
    /// </summary>
    public async Task<IReadOnlyList<Image>> GetImagesAsync(string url, CancellationToken ct = default)
    {
        ValidateUrl(url);
        return await _imageRepository.GetByUrlAsync(url, ct);
    }

    /// <summary>
    /// Downloads a video from a direct video URL to the specified path.
    /// </summary>
    /// <param name="videoUrl">Direct video download URL (from Video.PlayUrl).</param>
    /// <param name="destinationPath">Full file path where the video will be saved.</param>
    /// <param name="progress">Progress reporter (bytes downloaded).</param>
    /// <param name="ct">Cancellation token.</param>
    public async Task DownloadVideoAsync(
        string videoUrl,
        string destinationPath,
        IProgress<long>? progress = null,
        CancellationToken ct = default)
    {
        await _fileDownloader.DownloadFileAsync(videoUrl, destinationPath, progress, ct);
    }

    /// <summary>
    /// Downloads an image from a direct image URL to the specified path.
    /// </summary>
    /// <param name="imageUrl">Direct image download URL.</param>
    /// <param name="destinationPath">Full file path where the image will be saved.</param>
    /// <param name="progress">Progress reporter (bytes downloaded).</param>
    /// <param name="ct">Cancellation token.</param>
    public async Task DownloadImageAsync(
        string imageUrl,
        string destinationPath,
        IProgress<long>? progress = null,
        CancellationToken ct = default)
    {
        await _fileDownloader.DownloadFileAsync(imageUrl, destinationPath, progress, ct);
    }

    /// <summary>
    /// Downloads multiple images from their URLs to the specified directory.
    /// </summary>
    /// <param name="images">List of image metadata to download.</param>
    /// <param name="destinationDirectory">Directory where images will be saved.</param>
    /// <param name="progress">Progress reporter (bytes downloaded).</param>
    /// <param name="ct">Cancellation token.</param>
    public async Task DownloadImagesAsync(
        IReadOnlyList<Image> images,
        string destinationDirectory,
        IProgress<long>? progress = null,
        CancellationToken ct = default)
    {
        for (int i = 0; i < images.Count; i++)
        {
            ct.ThrowIfCancellationRequested();

            var image = images[i];
            var extension = Path.GetExtension(image.ImageUrl) ?? ".jpg";
            if (string.IsNullOrEmpty(extension) || extension.Contains('?'))
                extension = ".jpg";

            var fileName = $"{i + 1:D2}{extension}";
            var filePath = Path.Combine(destinationDirectory, fileName);

            await DownloadImageAsync(image.ImageUrl, filePath,
                new Progress<long>(p => progress?.Report(p)), ct);
        }
    }

    private static void ValidateUrl(string url)
    {
        if (!PublicationUrlValidator.IsValid(url))
            throw new ValidationException("Invalid TikTok URL. URL must be from tiktok.com or vm.tiktok.com.");
    }

}
