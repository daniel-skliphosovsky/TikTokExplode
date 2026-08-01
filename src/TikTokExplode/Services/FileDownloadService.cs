using System.Buffers;

namespace TikTokExplode;

/// <summary>
/// Downloads files over HTTP with progress reporting and cancellation support.
/// </summary>
internal sealed class FileDownloadService
{
    private readonly TikTokApiClient _apiClient;

    public FileDownloadService(TikTokApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task DownloadFileAsync(string url, string destinationPath, IProgress<double>? progress = null, CancellationToken ct = default)
    {
        try
        {
            using HttpResponseMessage response = await _apiClient.GetFileResponseAsync(url, ct).ConfigureAwait(false);
            using Stream source = await response.Content.ReadAsStreamAsync(ct).ConfigureAwait(false);
            using FileStream destination = File.Create(destinationPath);

            long totalLength = response.Content.Headers.ContentLength ?? 0;
            long totalRead = 0;

            byte[] buffer = ArrayPool<byte>.Shared.Rent(81920);
            try
            {
                while (true)
                {
                    int bytesRead = await source.ReadAsync(buffer.AsMemory(0, buffer.Length), ct).ConfigureAwait(false);
                    if (bytesRead <= 0)
                        break;

                    await destination.WriteAsync(buffer.AsMemory(0, bytesRead), ct).ConfigureAwait(false);

                    totalRead += bytesRead;
                    if (totalLength > 0)
                        progress?.Report((double)totalRead / totalLength);
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }
        catch (OperationCanceledException)
        {
            if (File.Exists(destinationPath))
                File.Delete(destinationPath);
            throw;
        }
    }
}
