using TikTokExplode.Domain.Entities;
using TikTokExplode.Domain.Interfaces;
using TikTokExplode.Infrastructure.Extraction;
using TikTokExplode.Infrastructure.Http;
using TikTokExplode.Infrastructure.Url;

namespace TikTokExplode.Infrastructure.Repositories;

public sealed class AuthorRepository : IAuthorRepository
{
    private readonly ITikTokApiClient _apiClient;
    private readonly IAuthorExtractor _authorExtractor;
    private readonly UrlHandler _urlHandler;

    public AuthorRepository(
        ITikTokApiClient apiClient,
        IAuthorExtractor authorExtractor,
        UrlHandler urlHandler)
    {
        _apiClient = apiClient;
        _authorExtractor = authorExtractor;
        _urlHandler = urlHandler;
    }

    public async Task<Author> GetByUrlAsync(string url, CancellationToken ct = default)
    {
        var fullUrl = await _urlHandler.GetFullUrlAsync(url, ct);
        var jsonResponse = await _apiClient.GetApiResponseAsync(fullUrl, ct);
        return _authorExtractor.ExtractAuthor(jsonResponse);
    }
}
