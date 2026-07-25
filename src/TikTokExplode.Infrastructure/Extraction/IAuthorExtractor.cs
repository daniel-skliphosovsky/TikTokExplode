using TikTokExplode.Domain.Entities;
using TikTokExplode.Infrastructure.DTOs;

namespace TikTokExplode.Infrastructure.Extraction;

public interface IAuthorExtractor
{
    Author ExtractAuthor(AuthorDto dto);
}
