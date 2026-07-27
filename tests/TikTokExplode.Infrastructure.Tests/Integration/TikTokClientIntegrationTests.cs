using System.Net;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TikTokExplode.Domain.Entities;
using TikTokExplode.Domain.Enums;
using TikTokExplode.Domain.Exceptions;
using TikTokExplode.Domain.Interfaces;
using TikTokExplode.Domain.Specifications;
using TikTokExplode.Extensions;
using TikTokExplode.Infrastructure.Configuration;
using TikTokExplode.Infrastructure.Download;
using TikTokExplode.Infrastructure.Extraction;
using TikTokExplode.Infrastructure.Http;
using TikTokExplode.Infrastructure.Repositories;
using TikTokExplode.Infrastructure.Url;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace TikTokExplode.Infrastructure.Tests.Integration;

public class TikTokClientIntegrationTests : IDisposable
{
    private readonly WireMockServer _mockServer;
    private readonly ITikTokClient _client;

    public TikTokClientIntegrationTests()
    {
        _mockServer = WireMockServer.Start();
        var mockServerUrl = _mockServer.Urls[0];

        var services = new ServiceCollection();

        // Configure options
        services.Configure<TikTokApiOptions>(options =>
        {
            options.BaseUrl = mockServerUrl;
            options.ApiUrl = mockServerUrl;
            options.TimeoutSeconds = 10;
            options.RetryCount = 1;
            options.RetryBaseDelayMs = 100;
        });

        // Register HTTP clients with redirect handler to point to WireMock
        services.AddHttpClient<ITikTokApiClient, TikTokApiClient>(client =>
        {
            client.BaseAddress = new Uri(mockServerUrl);
        })
        .AddHttpMessageHandler<HeadersHandler>()
        .AddHttpMessageHandler<RateLimitHandler>()
        .AddHttpMessageHandler(() => new MockServerRedirectHandler(mockServerUrl));

        services.AddHttpClient("TikTokApi", client =>
        {
            client.BaseAddress = new Uri(mockServerUrl);
        })
        .AddHttpMessageHandler<HeadersHandler>()
        .AddHttpMessageHandler(() => new MockServerRedirectHandler(mockServerUrl));

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

        var provider = services.BuildServiceProvider();
        _client = provider.GetRequiredService<ITikTokClient>();
    }

    [Fact]
    public async Task GetPublicationAsync_VideoUrl_ReturnsPublication()
    {
        // Arrange
        var videoUrl = "https://www.tiktok.com/@user/video/7412345678901234567";
        var apiResponse = File.ReadAllText(Path.Combine("Samples", "video_response.json"));

        // WireMock: handle HEAD (for UrlHandler) and GET (for TikTokApiClient)
        _mockServer
            .Given(Request.Create().WithPath("/@user/video/7412345678901234567").UsingHead())
            .RespondWith(Response.Create()
                .WithStatusCode(HttpStatusCode.OK));

        _mockServer
            .Given(Request.Create().WithPath("/@user/video/7412345678901234567").UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(HttpStatusCode.OK)
                .WithHeader("Content-Type", "application/json")
                .WithBody(apiResponse));

        // Act
        var publication = await _client.GetPublicationAsync(videoUrl);

        // Assert
        publication.Should().NotBeNull();
        publication.Id.Value.Should().Be(videoUrl);
        publication.Description.Should().Be("Test TikTok video");
        publication.Author.Should().NotBeNull();
        publication.Author.Nickname.Should().Be("TestUser");
        publication.Video.Should().NotBeNull();
        publication.Type.Should().Be(PublicationType.Video);
        publication.Soundtrack.Should().NotBeNull();
        publication.Stats.Should().NotBeNull();
    }

    [Fact]
    public async Task GetPublicationAsync_ImageUrl_ReturnsPublicationWithImages()
    {
        // Arrange
        var imageUrl = "https://www.tiktok.com/@user/photo/7412345678901234567";
        var apiResponse = File.ReadAllText(Path.Combine("Samples", "image_response.json"));

        _mockServer
            .Given(Request.Create().WithPath("/@user/photo/7412345678901234567").UsingHead())
            .RespondWith(Response.Create()
                .WithStatusCode(HttpStatusCode.OK));

        _mockServer
            .Given(Request.Create().WithPath("/@user/photo/7412345678901234567").UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(HttpStatusCode.OK)
                .WithHeader("Content-Type", "application/json")
                .WithBody(apiResponse));

        // Act
        var publication = await _client.GetPublicationAsync(imageUrl);

        // Assert
        publication.Should().NotBeNull();
        publication.Id.Value.Should().Be(imageUrl);
        publication.Type.Should().Be(PublicationType.Images);
        publication.Images.Should().NotBeNullOrEmpty();
        publication.Images.Should().HaveCount(2);
        publication.Video.Should().BeNull();
    }

    [Fact]
    public async Task GetPublicationAsync_InvalidUrl_ThrowsValidationException()
    {
        // Act & Assert
        await _client.Invoking(c => c.GetPublicationAsync("https://youtube.com/watch?v=123"))
            .Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task GetPublicationAsync_429Response_RetriesAndSucceeds()
    {
        // Arrange
        var videoUrl = "https://www.tiktok.com/@user/video/4291234567";
        var apiResponse = File.ReadAllText(Path.Combine("Samples", "video_response.json"));

        // HEAD request — always succeed
        _mockServer
            .Given(Request.Create().WithPath("/@user/video/4291234567").UsingHead())
            .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.OK));

        // GET request: first returns 429, transitions to "Retried" state
        _mockServer
            .Given(Request.Create().WithPath("/@user/video/4291234567").UsingGet())
            .InScenario("Rate limit then success")
            .WillSetStateTo("Retried")
            .RespondWith(Response.Create()
                .WithStatusCode(HttpStatusCode.TooManyRequests)
                .WithHeader("Retry-After", "0"));

        // After retry, return 200
        _mockServer
            .Given(Request.Create().WithPath("/@user/video/4291234567").UsingGet())
            .InScenario("Rate limit then success")
            .WhenStateIs("Retried")
            .RespondWith(Response.Create()
                .WithStatusCode(HttpStatusCode.OK)
                .WithHeader("Content-Type", "application/json")
                .WithBody(apiResponse));

        // Act
        var publication = await _client.GetPublicationAsync(videoUrl);

        // Assert
        publication.Should().NotBeNull();
        publication.Id.Value.Should().Be(videoUrl);
    }

    [Fact]
    public async Task GetPublicationAsync_NotFound_ThrowsException()
    {
        // Arrange
        var videoUrl = "https://www.tiktok.com/@user/video/notfound";

        _mockServer
            .Given(Request.Create().WithPath("/@user/video/notfound").UsingHead())
            .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.NotFound));

        _mockServer
            .Given(Request.Create().WithPath("/@user/video/notfound").UsingGet())
            .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.NotFound));

        // Act & Assert
        await _client.Invoking(c => c.GetPublicationAsync(videoUrl))
            .Should().ThrowAsync<Exception>();
    }

    public void Dispose()
    {
        _mockServer.Stop();
        _mockServer.Dispose();
    }
}
