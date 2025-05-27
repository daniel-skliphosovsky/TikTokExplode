using TikTokExplode.Publications;
using TikTokExplode.WebRequester;
using TikTokExplode.Exceptions;
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

        public async Task DownloadVideoAsync(Video video, string folderPath)
        {
            ValidateParameters(video, folderPath);
            EnsureDirectoryExists(folderPath);

            try
            {
                using HttpResponseMessage response = await _webRequestsHandler.GetDownloadUrlResponse(video.Url);

                string fileName = Path.Combine(folderPath, $"{video.AwemeId}.mp4");

                await using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
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

        public async Task DownloadImagesAsync(List<Image> images, string folderPath)
        {
            ValidateParameters(images, folderPath);

            if (!images.Any())
                return;

            string targetFolder = Path.Combine(folderPath, images.First().AwemeId);
            EnsureDirectoryExists(targetFolder);

            for (var i = 0; i < images.Count; i++)
            {
                try
                {
                    using HttpResponseMessage response = await _webRequestsHandler.GetDownloadUrlResponse(images[i].Url);
                    string fileName = Path.Combine(targetFolder, $"{images[i].AwemeId}_{i}.jpg");

                    await using FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                    await response.Content.CopyToAsync(fs);
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