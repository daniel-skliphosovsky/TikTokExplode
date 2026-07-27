using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using TikTokExplode.Domain.Interfaces;
using TikTokExplode.Domain.Specifications;
using TikTokExplode.Infrastructure.Configuration;
using TikTokExplode.Infrastructure.Download;
using TikTokExplode.Infrastructure.Extraction;
using TikTokExplode.Infrastructure.Http;
using TikTokExplode.Infrastructure.Repositories;
using TikTokExplode.Infrastructure.Url;

namespace TikTokExplode.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTikTokExplode(
        this IServiceCollection services,
        Action<TikTokApiOptions>? configureOptions = null)
    {
        if (configureOptions != null)
            services.Configure(configureOptions);
        else
            services.Configure<TikTokApiOptions>(_ => { });

        // Polly retry policy
        var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    // Logging can be added here
                });

        // Timeout policy
        var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(30));

        // Typed HttpClient with Polly policies
        services.AddHttpClient<ITikTokApiClient, TikTokApiClient>()
            .AddHttpMessageHandler<HeadersHandler>()
            .AddHttpMessageHandler<RateLimitHandler>()
            .AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(timeoutPolicy);

        // Named HttpClient for UrlHandler
        services.AddHttpClient("TikTokApi")
            .AddHttpMessageHandler<HeadersHandler>()
            .AddPolicyHandler(retryPolicy);

        // Handlers
        services.AddTransient<HeadersHandler>();
        services.AddTransient<RateLimitHandler>();

        // Infrastructure services
        services.AddSingleton<UrlHandler>();
        services.AddSingleton<IFileDownloader, HttpFileDownloader>();

        // Specifications
        services.AddSingleton<IPublicationUrlSpecification, PublicationUrlSpecification>();

        // Extractors
        services.AddSingleton<IVideoExtractor, VideoExtractor>();
        services.AddSingleton<IImageExtractor, ImageExtractor>();
        services.AddSingleton<IAuthorExtractor, AuthorExtractor>();
        services.AddSingleton<ISoundtrackExtractor, SoundtrackExtractor>();
        services.AddSingleton<IStatsExtractor, StatsExtractor>();

        // Repositories
        services.AddSingleton<IPublicationRepository, PublicationRepository>();
        services.AddSingleton<IVideoRepository, VideoRepository>();
        services.AddSingleton<IImageRepository, ImageRepository>();
        services.AddSingleton<IAuthorRepository, AuthorRepository>();
        services.AddSingleton<ISoundtrackRepository, SoundtrackRepository>();
        services.AddSingleton<IStatsRepository, StatsRepository>();

        // Facade
        services.AddSingleton<ITikTokClient, TikTokClient>();

        return services;
    }
}
