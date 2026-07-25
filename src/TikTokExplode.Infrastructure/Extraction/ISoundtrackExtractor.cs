using TikTokExplode.Domain.Entities;
using TikTokExplode.Infrastructure.DTOs;

namespace TikTokExplode.Infrastructure.Extraction;

public interface ISoundtrackExtractor
{
    Soundtrack? ExtractSoundtrack(MusicDto? dto);
}
