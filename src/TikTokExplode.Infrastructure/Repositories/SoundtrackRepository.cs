using TikTokExplode.Domain.Entities;
using TikTokExplode.Domain.Interfaces;
using TikTokExplode.Infrastructure.Extraction;
using TikTokExplode.Infrastructure.Http;
using TikTokExplode.Infrastructure.Url;

namespace TikTokExplode.Infrastructure.Repositories;

public sealed class SoundtrackRepository : ISoundtrackRepository
{
    private readonly ITikTokApiClient _apiClient;
    private readonly UrlHandler _urlHandler;
    private readonly ISoundtrackExtractor _extractor;

    public SoundtrackRepository(ITikTokApiClient apiClient, UrlHandler urlHandler, ISoundtrackExtractor extractor)
    {
        _apiClient = apiClient;
        _urlHandler = urlHandler;
        _extractor = extractor;
    }

    public async Task<Soundtrack> GetByUrlAsync(string url, CancellationToken ct = default)
    {
        var fullUrl = await _urlHandler.GetFullUrlAsync(url, ct);
        var json = await _apiClient.GetApiResponseAsync(fullUrl, ct);
        var aweme = ResponseParser.ParseFirstAweme(json);

        return _extractor.ExtractSoundtrack(aweme.Music)
            ?? throw new Domain.Exceptions.ValidationException("Failed to extract soundtrack data from response.");
    }
}
