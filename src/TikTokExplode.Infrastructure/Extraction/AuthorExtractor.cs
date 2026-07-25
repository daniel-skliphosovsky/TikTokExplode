using System.Text.Json;
using TikTokExplode.Domain.Entities;
using TikTokExplode.Domain.Exceptions;
using TikTokExplode.Domain.ValueObjects;

namespace TikTokExplode.Infrastructure.Extraction;

public sealed class AuthorExtractor : IAuthorExtractor
{
    public Author ExtractAuthor(string jsonResponse)
    {
        try
        {
            using var doc = JsonDocument.Parse(jsonResponse);
            var awemeList = doc.RootElement.GetProperty("aweme_list");

            if (awemeList.GetArrayLength() == 0)
                throw new ValidationException("No author data in response");

            var authorData = awemeList[0].GetProperty("author");

            var author = new Author
            {
                Id = AuthorId.Parse(authorData.GetProperty("uid").GetString() ?? string.Empty),
                Nickname = authorData.GetProperty("nickname").GetString() ?? string.Empty,
                IsVerified = authorData.GetProperty("is_star").GetBoolean(),
                ThumbAvatarUrl = authorData.GetProperty("avatar_thumb").GetProperty("url_list")[0].GetString() ?? string.Empty,
                MediumAvatarUrl = authorData.GetProperty("avatar_medium").GetProperty("url_list")[0].GetString() ?? string.Empty,
                Region = authorData.GetProperty("region").GetString() ?? string.Empty
            };

            return author;
        }
        catch (JsonException ex)
        {
            throw new ApiException("Failed to parse author JSON", 0, jsonResponse, ex);
        }
    }
}
