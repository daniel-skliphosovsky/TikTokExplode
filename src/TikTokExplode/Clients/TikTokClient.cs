using TikTokExplode.Exceptions;
using TikTokExplode.Publications;
using TikTokExplode.Publications.Images;
using TikTokExplode.Publications.Videos;

namespace TikTokExplode;

/// <summary>
/// Main facade for interacting with TikTok content.
/// Provides unified access to publication metadata and media downloading.
/// </summary>
public class TikTokClient : IDisposable
{
    /// <summary>
    /// Access to TikTok publications and their components.
    /// </summary>
    public PublicationClient Publications { get; }

    private readonly FileDownloadService _fileDownloader;
    private readonly HttpClient _httpClient;
    private readonly bool _ownsHttpClient;

    public TikTokClient(HttpClient? httpClient = null, TikTokClientOptions? options = null)
    {
        _ownsHttpClient = httpClient is null;
        _httpClient = httpClient ?? new HttpClient();

        var clientOptions = options ?? new TikTokClientOptions();
        if (_ownsHttpClient)
            _httpClient.Timeout = TimeSpan.FromSeconds(clientOptions.TimeoutSeconds);

        var apiClient = new TikTokApiClient(_httpClient, clientOptions);
        Publications = new PublicationClient(apiClient);
        _fileDownloader = new FileDownloadService(apiClient);
    }

    /// <summary>
    /// Downloads a publication video.
    /// </summary>
    /// <param name="video">Video metadata from <see cref="Publication"/> or <c>Publications.Videos</c>.</param>
    /// <param name="folderPath">Folder where the file will be saved.</param>
    /// <param name="customFileName">Optional file name without extension. Defaults to the publication id.</param>
    /// <param name="progress">Progress reporter (0 to 1).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task DownloadVideoAsync(
        Video video,
        string folderPath,
        string? customFileName = null,
        IProgress<double>? progress = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(video);

        await DownloadAsync(video.Url, folderPath, $"{customFileName ?? video.AwemeId}.mp4", progress, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Downloads a publication image.
    /// </summary>
    /// <param name="image">Image metadata from <see cref="Publication"/> or <c>Publications.Images</c>.</param>
    /// <param name="folderPath">Folder where the file will be saved.</param>
    /// <param name="customFileName">Optional file name without extension. Defaults to the publication id.</param>
    /// <param name="progress">Progress reporter (0 to 1).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task DownloadImageAsync(
        Image image,
        string folderPath,
        string? customFileName = null,
        IProgress<double>? progress = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(image);

        await DownloadAsync(image.Url, folderPath, $"{customFileName ?? image.AwemeId}.jpg", progress, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Downloads all images of a publication.
    /// </summary>
    /// <param name="images">Image metadata list from <see cref="Publication"/> or <c>Publications.Images</c>.</param>
    /// <param name="folderPath">Folder where the files will be saved.</param>
    /// <param name="customFileName">Optional file name prefix without extension. Defaults to the publication id.</param>
    /// <param name="progress">Progress reporter (0 to 1).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task DownloadImagesAsync(
        List<Image> images,
        string folderPath,
        string? customFileName = null,
        IProgress<double>? progress = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(images);

        if (images.Count == 0)
            throw new ValidationException("Images downloading error! Empty images list!");

        string fileName = customFileName ?? images[0].AwemeId;

        for (int i = 0; i < images.Count; i++)
        {
            await DownloadImageAsync(images[i], folderPath, $"{fileName}_{i + 1}", progress, cancellationToken).ConfigureAwait(false);
        }
    }

    public void Dispose()
    {
        if (_ownsHttpClient)
            _httpClient.Dispose();
    }

    private async Task DownloadAsync(
        string url,
        string folderPath,
        string fileName,
        IProgress<double>? progress,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(folderPath))
            throw new ArgumentException("Folder path cannot be empty.", nameof(folderPath));

        Directory.CreateDirectory(folderPath);

        await _fileDownloader.DownloadFileAsync(url, Path.Combine(folderPath, fileName), progress, ct).ConfigureAwait(false);
    }
}
