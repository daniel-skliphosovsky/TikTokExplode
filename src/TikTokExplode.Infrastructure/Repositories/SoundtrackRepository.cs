using TikTokExplode.Domain.Entities;
using TikTokExplode.Domain.Interfaces;
using TikTokExplode.Infrastructure.Extraction;
using TikTokExplode.Infrastructure.Http;
using TikTokExplode.Infrastructure.Url;

namespace TikTokExplode.Infrastructure.Repositories;

public sealed class SoundtrackRepository : ISoundtrackRepository
{
    private readonly ITikTokApiClient _apiClient;
    private readonly ISoundtrackExtractor _soundtrackExtractor;
    private readonly UrlHandler _urlHandler;

    public SoundtrackRepository(
        ITikTokApiClient apiClient,
        ISoundtrackExtractor soundtrackExtractor,
        UrlHandler urlHandler)
    {
        _apiClient = apiClient;
        _soundtrackExtractor = soundtrackExtractor;
        _urlHandler = urlHandler;
    }

    public async Task<Soundtrack> GetByUrlAsync(string url, CancellationToken ct = default)
    {
        var fullUrl = await _urlHandler.GetFullUrlAsync(url, ct);
        var jsonResponse = await _apiClient.GetApiResponseAsync(fullUrl, ct);
        return _soundtrackExtractor.ExtractSoundtrack(jsonResponse);
    }
}
