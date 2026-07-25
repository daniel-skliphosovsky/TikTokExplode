using TikTokExplode.Domain.Entities;
using TikTokExplode.Domain.ValueObjects;
using TikTokExplode.Infrastructure.DTOs;

namespace TikTokExplode.Infrastructure.Extraction;

public class AuthorExtractor : IAuthorExtractor
{
    public Author ExtractAuthor(AuthorDto dto)
    {
        return new Author
        {
            Id = new AuthorId(dto.Uid),
            Nickname = dto.Nickname,
            IsVerified = dto.IsStar,
            ThumbAvatarUrl = dto.AvatarThumb?.UrlList?.FirstOrDefault() ?? string.Empty,
            MediumAvatarUrl = dto.AvatarMedium?.UrlList?.FirstOrDefault() ?? string.Empty,
            Region = dto.Region
        };
    }
}
