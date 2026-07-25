using System.Buffers;
using TikTokExplode.Domain.Exceptions;
using TikTokExplode.Infrastructure.Http;

namespace TikTokExplode.Infrastructure.Download;

public sealed class FileDownloadService
{
    private readonly ITikTokApiClient _apiClient;

    public FileDownloadService(ITikTokApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task DownloadFileAsync(
        string url,
        string destinationPath,
        IProgress<double>? progress = null,
        CancellationToken ct = default)
    {
        try
        {
            var fileInfo = new FileInfo(destinationPath);
            if (fileInfo.Directory != null && !fileInfo.Directory.Exists)
                fileInfo.Directory.Create();

            using var stream = await _apiClient.GetStreamAsync(url, ct);
            using var fileStream = File.Create(destinationPath);
            
            var pool = ArrayPool<byte>.Shared;
            var buffer = pool.Rent(81920); // 80 KB buffer

            try
            {
                long totalRead = 0;
                int bytesRead;
                
                while ((bytesRead = await stream.ReadAsync(buffer.AsMemory(0, buffer.Length), ct)) > 0)
                {
                    await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead), ct);
                    totalRead += bytesRead;
                    progress?.Report(1.0 * totalRead / (stream.Length > 0 ? stream.Length : totalRead));
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }
        catch (OperationCanceledException)
        {
            // Delete incomplete file on cancellation
            if (File.Exists(destinationPath))
                File.Delete(destinationPath);
            throw;
        }
        catch (Exception ex) when (ex is not TikTokExplodeException)
        {
            throw new ApiException("Failed to download file", 0, ex.Message, ex);
        }
    }
}
