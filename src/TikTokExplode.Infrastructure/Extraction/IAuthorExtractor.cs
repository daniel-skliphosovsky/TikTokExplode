using TikTokExplode.Domain.Entities;

namespace TikTokExplode.Infrastructure.Extraction;

public interface IAuthorExtractor
{
    Author ExtractAuthor(string jsonResponse);
}
