<h1 align="center">TikTokExplode</h1>
<p align="center">.NET library for interacting with TikTok content. Extract metadata and download videos, images, and music from TikTok publications.</p>
<p align="center">
  <a href="https://github.com/daniel-skliphosovsky/TikTokExplode/actions/workflows/ci.yml"><img src="https://github.com/daniel-skliphosovsky/TikTokExplode/actions/workflows/ci.yml/badge.svg" alt="CI"></a>
  <a href="https://github.com/daniel-skliphosovsky/TikTokExplode/blob/main/LICENSE"><img src="https://img.shields.io/github/license/daniel-skliphosovsky/TikTokExplode" alt="MIT License"></a>
  <a href="https://github.com/daniel-skliphosovsky/TikTokExplode/releases"><img src="https://img.shields.io/github/v/release/daniel-skliphosovsky/TikTokExplode" alt="Release"></a>
</p>

<h6 align="center">TikTok API uses rate limiting. This library does not bypass it. If you get empty responses, wait a few minutes and try again.</h6>

## Installation

Download the latest `TikTokExplode.dll` from the [Releases](https://github.com/daniel-skliphosovsky/TikTokExplode/releases) page.

Add a reference to the DLL in your project:

```xml
<Reference Include="TikTokExplode">
  <HintPath>path\to\TikTokExplode.dll</HintPath>
</Reference>
```

## Dependency Injection

```csharp
using TikTokExplode.Extensions;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddTikTokExplode(options =>
{
    options.TimeoutSeconds = 60;
    options.MaxRetries = 5;
});

var provider = services.BuildServiceProvider();
var Client = provider.GetRequiredService<TikTokExplode.TikTokClient>();
```

## Usage

### Get publication metadata (author, video/images, soundtrack, stats)

```csharp
using TikTokExplode;
using TikTokExplode.Extensions;
using Microsoft.Extensions.DependencyInjection;

var provider = new ServiceCollection()
    .AddTikTokExplode()
    .BuildServiceProvider();

var Client = provider.GetRequiredService<TikTokClient>();

var Publication = await Client.GetPublicationAsync("publication_url");
Console.WriteLine(Publication.Description);
Console.WriteLine($"Author: {Publication.Author.Nickname}");
Console.WriteLine($"Likes: {Publication.Stats.DiggCount}");
```

### Get video metadata

```csharp
var Video = await Client.GetVideoAsync("publication_url");
Console.WriteLine($"Duration: {Video.Duration}s");
Console.WriteLine($"Resolution: {Video.Width}x{Video.Height}");
```

### Get image metadata

```csharp
var Images = await Client.GetImagesAsync("publication_url");
Console.WriteLine($"Images count: {Images.Count}");
```

### Download video

```csharp
using TikTokExplode;
using TikTokExplode.Extensions;
using Microsoft.Extensions.DependencyInjection;

var Client = new ServiceCollection()
    .AddTikTokExplode()
    .BuildServiceProvider()
    .GetRequiredService<TikTokClient>();

var Video = await Client.GetVideoAsync("publication_url");
await Client.DownloadVideoAsync(Video.PlayUrl, "video.mp4", Progress =>
{
    Console.WriteLine($"Downloaded: {Progress:P0}");
});
```

### Download image

```csharp
var Publication = await Client.GetPublicationAsync("publication_url");

if (Publication.Type == TikTokExplode.Domain.Enums.PublicationType.Images && Publication.Images != null)
{
    await Client.DownloadImagesAsync(Publication.Images, "./images", P =>
    {
        Console.WriteLine($"Total progress: {P:P0}");
    });
}
```

### Download with cancellation

```csharp
using var Cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

try
{
    await Client.DownloadVideoAsync(Video.PlayUrl, "video.mp4", null, Cts.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Download cancelled or timed out");
}
```

### Get author, soundtrack, stats separately

```csharp
var Publication = await Client.GetPublicationAsync("publication_url");

// Author
Console.WriteLine($"Author: {Publication.Author.Nickname} ({Publication.Author.Region})");
Console.WriteLine($"Verified: {Publication.Author.IsVerified}");

// Soundtrack
Console.WriteLine($"Music: {Publication.Soundtrack.Title} by {Publication.Soundtrack.AuthorName}");

// Stats
Console.WriteLine($"Plays: {Publication.Stats.PlayCount}");
Console.WriteLine($"Likes: {Publication.Stats.DiggCount}");
Console.WriteLine($"Shares: {Publication.Stats.ShareCount}");
```

## Architecture

The library is organized into three layers:

- **TikTokExplode.Domain** — entities, value objects, interfaces, exceptions. No external dependencies.
- **TikTokExplode.Infrastructure** — HTTP client, JSON extractors, file downloader, repositories.
- **TikTokExplode** (facade) — public API, DI registration.

## Possible problems

- **Rate limiting**: TikTok API may return empty responses if too many requests are made in a short time. Wait a few minutes and try again.
- **Broken videos/images**: Sometimes TikTok API returns a different video/image than the one requested. This is a known TikTok API issue.
- **Redirects**: TikTok uses URL shorteners (vm.tiktok.com). The library follows redirects automatically.
- **Region restrictions**: Some content may not be accessible from certain regions.

## Build from source

```bash
git clone https://github.com/daniel-skliphosovsky/TikTokExplode.git
cd TikTokExplode
dotnet restore
dotnet build
dotnet test
```

## License

MIT
