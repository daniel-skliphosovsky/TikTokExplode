using TikTokExplode.Domain.Entities;
using TikTokExplode.Domain.Exceptions;
using TikTokExplode.Domain.Interfaces;
using TikTokExplode.Infrastructure.Extraction;
using TikTokExplode.Infrastructure.Http;
using TikTokExplode.Infrastructure.Url;

namespace TikTokExplode.Infrastructure.Repositories;

public sealed class StatsRepository : IStatsRepository
{
    private readonly ITikTokApiClient _apiClient;
    private readonly UrlHandler _urlHandler;
    private readonly IStatsExtractor _extractor;

    public StatsRepository(ITikTokApiClient apiClient, UrlHandler urlHandler, IStatsExtractor extractor)
    {
        _apiClient = apiClient;
        _urlHandler = urlHandler;
        _extractor = extractor;
    }

    public async Task<Stats> GetByUrlAsync(string url, CancellationToken ct = default)
    {
        var fullUrl = await _urlHandler.GetFullUrlAsync(url, ct);
        var json = await _apiClient.GetApiResponseAsync(fullUrl, ct);
        var aweme = ResponseParser.ParseFirstAweme(json);

        return _extractor.ExtractStats(aweme.Statistics
            ?? throw new ValidationException("Failed to extract statistics data from response."));
    }
}
