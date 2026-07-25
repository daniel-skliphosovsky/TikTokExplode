using TikTokExplode.Domain.Entities;
using TikTokExplode.Domain.Exceptions;
using TikTokExplode.Domain.Interfaces;
using TikTokExplode.Infrastructure.Extraction;
using TikTokExplode.Infrastructure.Http;
using TikTokExplode.Infrastructure.Url;

namespace TikTokExplode.Infrastructure.Repositories;

public sealed class AuthorRepository : IAuthorRepository
{
    private readonly ITikTokApiClient _apiClient;
    private readonly UrlHandler _urlHandler;
    private readonly IAuthorExtractor _extractor;

    public AuthorRepository(ITikTokApiClient apiClient, UrlHandler urlHandler, IAuthorExtractor extractor)
    {
        _apiClient = apiClient;
        _urlHandler = urlHandler;
        _extractor = extractor;
    }

    public async Task<Author> GetByUrlAsync(string url, CancellationToken ct = default)
    {
        var fullUrl = await _urlHandler.GetFullUrlAsync(url, ct);
        var json = await _apiClient.GetApiResponseAsync(fullUrl, ct);
        var aweme = ResponseParser.ParseFirstAweme(json);

        return _extractor.ExtractAuthor(aweme.Author
            ?? throw new ValidationException("Failed to extract author data from response."));
    }
}
