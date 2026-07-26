# TikTokExplode

A .NET library for extracting metadata and downloading content from TikTok without official API.

## Features

- Extract full publication metadata (author, video/images, soundtrack, stats)
- Get video and image metadata separately
- Download videos, single images, or multiple images
- Progress reporting for downloads
- Cancellation token support for all operations
- Configurable HTTP client via dependency injection
- Automatic handling of redirects and URL shorteners

## Requirements

- .NET 10.0+

## Installation

**NuGet** (if published):
```
dotnet add package TikTokExplode
```

**Direct DLL reference:**
Download the latest `TikTokExplode.dll` from the [Releases](https://github.com/daniel-skliphosovsky/TikTokExplode/releases) page and add a reference:

```xml
<Reference Include="TikTokExplode">
  <HintPath>path\to\TikTokExplode.dll</HintPath>
</Reference>
```

## Quick Usage

```csharp
using TikTokExplode;
using TikTokExplode.Extensions;
using Microsoft.Extensions.DependencyInjection;

var client = new ServiceCollection()
    .AddTikTokExplode()
    .BuildServiceProvider()
    .GetRequiredService<ITikTokClient>();

var publication = await client.GetPublicationAsync("https://vm.tiktok.com/XXXXXX");
Console.WriteLine($"Author: {publication.Author.Nickname}");
Console.WriteLine($"Description: {publication.Description}");
Console.WriteLine($"Likes: {publication.Stats.DiggCount}");
```

## API Reference

### ITikTokClient

| Method | Description |
|--------|-------------|
| `GetPublicationAsync(string url, CancellationToken ct)` | Gets full publication metadata (author, video/images, soundtrack, stats) |
| `GetVideoAsync(string url, CancellationToken ct)` | Gets video metadata |
| `GetImagesAsync(string url, CancellationToken ct)` | Gets image metadata |
| `DownloadVideoAsync(string videoUrl, string destinationPath, IProgress<long>?, CancellationToken ct)` | Downloads a video from a direct URL to a file |
| `DownloadImageAsync(string imageUrl, string destinationPath, IProgress<long>?, CancellationToken ct)` | Downloads a single image from a direct URL to a file |
| `DownloadImagesAsync(IReadOnlyList<Image> images, string destinationDir, IProgress<long>?, CancellationToken ct)` | Downloads multiple images to a directory |

### DI Registration

```csharp
services.AddTikTokExplode(options =>
{
    options.TimeoutSeconds = 60;
    options.MaxRetries = 5;
});
```

## Project Structure

- **TikTokExplode.Domain** -- entities, value objects, interfaces, exceptions. No external dependencies.
- **TikTokExplode.Infrastructure** -- HTTP client, JSON extractors, file downloader, repositories.
- **TikTokExplode** (facade) -- public API (`ITikTokClient`, `TikTokClient`), DI registration.

## Build

```bash
git clone https://github.com/daniel-skliphosovsky/TikTokExplode.git
cd TikTokExplode
dotnet restore
dotnet build
dotnet test
```

## License

This project is licensed under the MIT License.
