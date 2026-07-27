using System.Buffers;
using System.IO;
using TikTokExplode.Domain.Exceptions;
using TikTokExplode.Domain.Interfaces;
using TikTokExplode.Infrastructure.Http;

namespace TikTokExplode.Infrastructure.Download;

public sealed class HttpFileDownloader : IFileDownloader
{
    private readonly ITikTokApiClient _apiClient;

    public HttpFileDownloader(ITikTokApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task DownloadFileAsync(
        string url,
        string destinationPath,
        IProgress<long>? progress = null,
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
            var buffer = pool.Rent(81920);

            try
            {
                long totalRead = 0;
                int bytesRead;

                while ((bytesRead = await stream.ReadAsync(buffer.AsMemory(0, buffer.Length), ct)) > 0)
                {
                    await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead), ct);
                    totalRead += bytesRead;
                    progress?.Report(totalRead);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }
        catch (OperationCanceledException)
        {
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
