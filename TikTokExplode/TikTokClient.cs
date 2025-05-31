using TikTokExplode.Exceptions;
using TikTokExplode.Extensions;
using TikTokExplode.Publications;
using TikTokExplode.WebRequester;
using TikTokExplode.Publications.Videos;
using TikTokExplode.Publications.Images;

namespace TikTokExplode
{
    public class TikTokClient
    {
        public PublicationClient Publications { get; }

        private readonly WebRequestsHandler _webRequestsHandler;

        public TikTokClient()
        {
            Publications = new PublicationClient();
            _webRequestsHandler = new WebRequestsHandler();
        }

        /// <summary>
        /// Download publication video
        /// </summary>
        public async Task DownloadVideoAsync(Video video,
                                             string folderPath,
                                             string customFileName = null,
                                             IProgress<double> progress = null,
                                             CancellationToken cancellationToken = default)
        {
            ValidateParameters(video, folderPath);
            EnsureDirectoryExists(folderPath);

            try
            {
                using HttpResponseMessage response = await _webRequestsHandler.GetDownloadUrlResponse(video.Url);

                string fileName = Path.Combine(folderPath, $"{customFileName ?? video.AwemeId}.mp4");

                using FileStream destination = File.Create(fileName);

                long totalLength = response.Content.Headers.ContentLength ?? 0;

                Stream stream = await response.Content.ReadAsStreamAsync();

                await stream.CopyToAsync(
                    destination,
                    totalLength,
                    progress,
                    cancellationToken: cancellationToken);
            }
            catch (TikTokExplodeException)
            {
                throw;
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                throw new TikTokExplodeException("Video downloading error!" + ex);
            }
        }

        /// <summary>
        /// Download publication image
        /// </summary>
        public async Task DownloadImageAsync(Image image,
                                             string folderPath,
                                             string customFileName = null,
                                             IProgress<double> progress = null,
                                             CancellationToken cancellationToken = default)
        {
            ValidateParameters(image, folderPath);
            EnsureDirectoryExists(folderPath);

            try
            {
                using HttpResponseMessage response = await _webRequestsHandler.GetDownloadUrlResponse(image.Url);

                string fileName = Path.Combine(folderPath, $"{customFileName ?? image.AwemeId}.mp4");

                using FileStream destination = File.Create(fileName);

                long totalLength = response.Content.Headers.ContentLength ?? 0;

                Stream stream = await response.Content.ReadAsStreamAsync();

                await stream.CopyToAsync(
                    destination,
                    totalLength,
                    progress,
                    cancellationToken: cancellationToken);
            }
            catch (TikTokExplodeException)
            {
                throw;
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                throw new TikTokExplodeException("Image downloading error!" + ex);
            }
        }

        /// <summary>
        /// Download publication images
        /// </summary>
        public async Task DownloadImagesAsync(List<Image> images,
                                             string folderPath,
                                             string customFileName = null,
                                             IProgress<double> progress = null,
                                             CancellationToken cancellationToken = default)
        {

            if (!images.Any())
                throw new TikTokExplodeException("Images downloading error! Empty images list!");

            
            for (int i = 0; i < images.Count(); i++)
            {
                await DownloadImageAsync(images[i], folderPath, customFileName, progress, cancellationToken);
            }
        }

        private static void ValidateParameters(object obj, string folderPath)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj), "Downloading error! Object null reference");

            if (string.IsNullOrWhiteSpace(folderPath))
                throw new ArgumentException("Folder path cannot be empty", nameof(folderPath));

            if (Path.GetDirectoryName(folderPath) is null)
                throw new ArgumentException("Invalid folder path", nameof(folderPath));
        }

        private static void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}