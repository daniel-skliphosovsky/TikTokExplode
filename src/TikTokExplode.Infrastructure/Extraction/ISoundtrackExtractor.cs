using TikTokExplode.Domain.Entities;

namespace TikTokExplode.Infrastructure.Extraction;

public interface ISoundtrackExtractor
{
    Soundtrack ExtractSoundtrack(string jsonResponse);
}
