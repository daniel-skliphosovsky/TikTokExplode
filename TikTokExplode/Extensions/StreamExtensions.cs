using System.Buffers;

namespace TikTokExplode.Extensions
{
    internal static class StreamExtensions
    {
        public static async ValueTask CopyToAsync(
            this Stream source,
            Stream destination,
            long totalLength,
            IProgress<double>? progress = null,
            int bufferSize = 0x1000,
            CancellationToken cancellationToken = default
        )
        {
            using IMemoryOwner<byte> buffer = MemoryPool<byte>.Shared.Rent(bufferSize);

            long totalBytesRead = 0L;
            while (true)
            {
                int bytesRead = await source.ReadAsync(buffer.Memory, cancellationToken);
                if (bytesRead <= 0)
                    break;

                await destination.WriteAsync(buffer.Memory[..bytesRead], cancellationToken);

                totalBytesRead += bytesRead;
                progress?.Report(1.0 * totalBytesRead / totalLength);
            }
        }
    }
}

