using TikTokExplode.Exceptions;
using TikTokExplode.Publications;
using TikTokExplode.WebRequester;
using TikTokExplode.Utils.Extensions;
using TikTokExplode.Publications.Videos;
using TikTokExplode.Publications.Images;
using System;
using System.IO;
using System.Threading;


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

        public async Task DownloadVideoAsync(Video video, string filePath, IProgress<double>? progress = null)
        {
            ValidateParameters(video, filePath);
            EnsureDirectoryExists(filePath);

            try
            {
                using FileStream destination = new FileStream(filePath, FileMode.Create, FileAccess.Write);

                using HttpResponseMessage response = await _webRequestsHandler.GetDownloadUrlResponse(video.Url);

                long totalLength = response.Content.Headers.ContentLength ?? 0;

                using Stream stream = await response.Content.ReadAsStreamAsync();

                await stream.CopyToAsync(destination, totalLength, progress);
            }
            catch (TikTokExplodeException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException("Video downloading error!" + ex);
            }
        }

        public async Task DownloadImageAsync(Image image, string filePath, IProgress<double>? progress = null)
        {
            ValidateParameters(image, filePath);
            EnsureDirectoryExists(filePath);

            try
            {

                using FileStream destination = new FileStream(filePath, FileMode.Create, FileAccess.Write);

                using HttpResponseMessage response = await _webRequestsHandler.GetDownloadUrlResponse(image.Url);

                long totalLength = response.Content.Headers.ContentLength ?? 0;

                using Stream stream = await response.Content.ReadAsStreamAsync();

                await stream.CopyToAsync(destination, totalLength, progress);
            }
            catch (TikTokExplodeException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException("Image downloading error!" + ex);
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