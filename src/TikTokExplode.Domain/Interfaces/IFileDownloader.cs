using System.IO;

namespace TikTokExplode.Domain.Interfaces;

public interface IFileDownloader
{
    Task DownloadFileAsync(
        string url,
        string destinationPath,
        IProgress<long>? progress = null,
        CancellationToken ct = default);
}
