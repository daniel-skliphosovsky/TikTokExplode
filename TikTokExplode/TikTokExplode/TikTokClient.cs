using TikTokExplode.Exceptions;
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
        public async Task DownloadVideoAsync(Video video, string folderPath, string customFileName = null, IProgress<double>? progress = null)
        {
            ValidateParameters(video, folderPath);
            EnsureDirectoryExists(folderPath);

            try
            {
                using HttpResponseMessage response = await _webRequestsHandler.GetDownloadUrlResponse(video.Url);

                string fileName = Path.Combine(folderPath, $"{customFileName ?? video.AwemeId}.mp4");

                using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    await response.Content.CopyToAsync(fs);
                }
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

        /// <summary>
        /// Download publication image
        /// </summary>
        public async Task DownloadImagesAsync(List<Image> images, string folderPath, string customFileName = null, IProgress<double>? progress = null)
        {
            ValidateParameters(images, folderPath);
            EnsureDirectoryExists(folderPath);

            if (!images.Any())
                throw new TikTokExplodeException("Images downloading error! Empty images list!");

            try
            {
                for (int i = 0; i < images.Count(); i++)
                {
                    using HttpResponseMessage response = await _webRequestsHandler.GetDownloadUrlResponse(images[i].Url);

                    string fileName = Path.Combine(folderPath, $"{customFileName ?? images[i].AwemeId}_{i}.jpg");

                    using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                    {
                        await response.Content.CopyToAsync(fs);
                    }
                }
            }
            catch (TikTokExplodeException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new TikTokExplodeException("Images downloading error!" + ex);
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