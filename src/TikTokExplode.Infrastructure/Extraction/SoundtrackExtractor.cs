using TikTokExplode.Domain.Entities;
using TikTokExplode.Domain.ValueObjects;
using TikTokExplode.Infrastructure.DTOs;

namespace TikTokExplode.Infrastructure.Extraction;

public class SoundtrackExtractor : ISoundtrackExtractor
{
    public Soundtrack? ExtractSoundtrack(MusicDto? dto)
    {
        if (dto is null)
            return null;

        return new Soundtrack
        {
            Id = new SoundtrackId(dto.Id),
            Title = dto.Title,
            AuthorName = dto.AuthorName,
            SoundUrl = dto.PlayUrl?.UrlList?.FirstOrDefault() ?? string.Empty,
            LargeCoverUrl = dto.CoverLarge?.UrlList?.FirstOrDefault() ?? string.Empty,
            MediumCoverUrl = dto.CoverMedium?.UrlList?.FirstOrDefault() ?? string.Empty,
            ThumbCoverUrl = dto.CoverThumb?.UrlList?.FirstOrDefault() ?? string.Empty
        };
    }
}
