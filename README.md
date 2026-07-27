# TikTokExplode

A .NET library for extracting metadata and downloading content from TikTok without official API.

[![CI](https://github.com/daniel-skliphosovsky/TikTokExplode/actions/workflows/ci.yml/badge.svg)](https://github.com/daniel-skliphosovsky/TikTokExplode/actions/workflows/ci.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## Features

- Extract full publication metadata (author, video/images, soundtrack, stats)
- Get video and image metadata separately
- Download videos, single images, or multiple images
- Progress reporting for downloads
- Cancellation token support for all operations
- Configurable HTTP client via dependency injection
- Automatic handling of redirects and URL shorteners
- Rate limiting and retry with exponential backoff

## Requirements

- .NET 9.0+

## Installation

**NuGet** (if published):
```
dotnet add package TikTokExplode
```

**Direct DLL reference:**
Download the latest release DLLs from the [Releases](https://github.com/daniel-skliphosovsky/TikTokExplode/releases) page and add a reference:

```xml
<Reference Include="TikTokExplode">
  <HintPath>path\to\TikTokExplode.dll</HintPath>
</Reference>
```

## Quick Start

```csharp
using TikTokExplode;
using TikTokExplode.Extensions;
using Microsoft.Extensions.DependencyInjection;

// Setup DI
var services = new ServiceCollection();
services.AddTikTokExplode();
var provider = services.BuildServiceProvider();

// Get client
var client = provider.GetRequiredService<ITikTokClient>();

// Get publication metadata
var publication = await client.GetPublicationAsync("https://www.tiktok.com/@user/video/1234567890");

// Download video
await client.DownloadVideoAsync(publication, "video.mp4");
```

## Configuration

### TikTokApiOptions

| Property | Default | Description |
|----------|---------|-------------|
| `BaseUrl` | `https://www.tiktok.com` | Base URL for TikTok web |
| `ApiUrl` | `https://api22-normal-c-alisg.tiktokv.com` | API endpoint |
| `UserAgents` | 4 Chrome UAs | User agent rotation pool |
| `TimeoutSeconds` | 30 | HTTP request timeout |
| `RetryCount` | 3 | Polly retry count |
| `RetryBaseDelayMs` | 1000 | Base delay for exponential backoff |

### Advanced DI Registration

```csharp
services.AddTikTokExplode(options =>
{
    options.TimeoutSeconds = 60;
    options.RetryCount = 5;
    options.RetryBaseDelayMs = 2000;
});
```

## Architecture

```
TikTokExplode.Domain          -> Entities, Interfaces, Specifications, Exceptions
TikTokExplode.Infrastructure  -> HTTP (Polly, Typed Client), Extractors, Repositories, Download
TikTokExplode                 -> Facade (ITikTokClient), DI Extensions
```

## Building from Source

```bash
git clone https://github.com/daniel-skliphosovsky/TikTokExplode.git
cd TikTokExplode
dotnet restore
dotnet build -c Release
dotnet test -c Release
dotnet pack -c Release
```

## License

MIT
