using TikTokExplode.Domain.Entities;
using TikTokExplode.Domain.Interfaces;
using TikTokExplode.Infrastructure.Extraction;
using TikTokExplode.Infrastructure.Http;
using TikTokExplode.Infrastructure.Url;

namespace TikTokExplode.Infrastructure.Repositories;

public sealed class StatsRepository : IStatsRepository
{
    private readonly ITikTokApiClient _apiClient;
    private readonly IStatsExtractor _statsExtractor;
    private readonly UrlHandler _urlHandler;

    public StatsRepository(
        ITikTokApiClient apiClient,
        IStatsExtractor statsExtractor,
        UrlHandler urlHandler)
    {
        _apiClient = apiClient;
        _statsExtractor = statsExtractor;
        _urlHandler = urlHandler;
    }

    public async Task<Stats> GetByUrlAsync(string url, CancellationToken ct = default)
    {
        var fullUrl = await _urlHandler.GetFullUrlAsync(url, ct);
        var jsonResponse = await _apiClient.GetApiResponseAsync(fullUrl, ct);
        return _statsExtractor.ExtractStats(jsonResponse);
    }
}
