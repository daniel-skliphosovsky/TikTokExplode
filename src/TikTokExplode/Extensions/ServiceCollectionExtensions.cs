using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TikTokExplode.Domain.Interfaces;
using TikTokExplode.Domain.Specifications;
using TikTokExplode.Infrastructure.Configuration;
using TikTokExplode.Infrastructure.Download;
using TikTokExplode.Infrastructure.Extraction;
using TikTokExplode.Infrastructure.Http;
using TikTokExplode.Infrastructure.Repositories;
using TikTokExplode.Infrastructure.Url;

namespace TikTokExplode.Extensions;

/// <summary>
/// Extension methods for registering TikTokExplode services in DI container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds TikTokExplode services to the specified IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <param name="configureOptions">Optional action to configure TikTokApiOptions.</param>
    /// <returns>The IServiceCollection so that additional calls can be chained.</returns>
    public static IServiceCollection AddTikTokExplode(
        this IServiceCollection services,
        Action<TikTokApiOptions>? configureOptions = null)
    {
        // Configure options
        if (configureOptions != null)
            services.Configure(configureOptions);
        else
            services.Configure<TikTokApiOptions>(_ => { });

        // HTTP client
        services.AddHttpClient("TikTokApi")
            .ConfigureHttpClient((sp, client) =>
            {
                var options = sp.GetRequiredService<IOptions<TikTokApiOptions>>().Value;
                client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
                client.DefaultRequestHeaders.Connection.ParseAdd("keep-alive");
            });

        // Domain services
        services.AddSingleton<IPublicationUrlSpecification, PublicationUrlSpecification>();

        // Infrastructure services
        services.AddSingleton<HeadersProvider>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<TikTokApiOptions>>().Value;
            return new HeadersProvider(options.UserAgents);
        });
        services.AddSingleton<UrlHandler>();
        services.AddSingleton<ITikTokApiClient, TikTokApiClient>();
        services.AddSingleton<IFileDownloader, HttpFileDownloader>();

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
