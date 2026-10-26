<h1 align="center">
    TikTokExplode
</h1>

<h3 align="center">
    .NET library for TikTok API interaction
</h3>
<h6 align="center">
    Note: Due to TikTok API's rate limiting, download functions or object fetching operations may take up to 10 seconds to complete.
</h6>

<p align="center">
    <a href="https://github.com/daniel-skliphosovsky/TikTokExplode/actions/workflows/ci.yml">
      <img src="https://img.shields.io/badge/CI-Passing-brightgreen?style=for-the-badge&logo=github" alt="CI">
    </a>
    <a href="https://github.com/daniel-skliphosovsky/TikTokExplode/blob/main/LICENSE">
      <img src="https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge" alt="License">
    </a>
</p>

## Description

TikTokExplode is a .NET library that provides access to TikTok's API for retrieving publication metadata, author information, statistics, soundtracks, videos, and images. It handles short-link resolution, retries with fresh user agents, and supports downloading content locally.

## Installation

Download `TikTokExplode.dll` from the [latest GitHub release](https://github.com/daniel-skliphosovsky/TikTokExplode/releases/latest) and add it as a reference in your project.

For an SDK-style project, add the reference to your `.csproj`:

```xml
<ItemGroup>
  <Reference Include="TikTokExplode">
    <HintPath>path/to/TikTokExplode.dll</HintPath>
  </Reference>
</ItemGroup>
```

### Add namespace

```csharp
using TikTokExplode;
```

## Usage

### Basic client setup

```csharp
TikTokClient client = new TikTokClient();
```

### Get publication data

```csharp
using TikTokExplode;
using TikTokExplode.Publications;

TikTokClient client = new TikTokClient();
Publication publication = await client.Publications.GetAsync("https://www.tiktok.com/@user/video/123456");
```

### Download video

```csharp
using TikTokExplode;
using TikTokExplode.Publications.Videos;

TikTokClient client = new TikTokClient();
Video video = await client.Publications.Videos.GetAsync("https://www.tiktok.com/@user/video/123456");
await client.DownloadVideoAsync(video, "/path/to/save", "video.mp4");
```

### Download images

```csharp
using TikTokExplode;
using TikTokExplode.Publications.Images;

TikTokClient client = new TikTokClient();
List<Image> images = await client.Publications.Images.GetAsync("https://www.tiktok.com/@user/photo/123456");
await client.DownloadImagesAsync(images, "/path/to/save");
```

### Check publication type

```csharp
using TikTokExplode;
using TikTokExplode.Publications;

TikTokClient client = new TikTokClient();
string url = "https://www.tiktok.com/@user/video/123456";
PublicationType type = await PublicationClient.GetPublicationType(url);
```

## Contributing

See [CONTRIBUTING.md](./CONTRIBUTING.md) for contribution guidelines.

## Possible problems

If the post you linked to is private (or does not exist), the program will download another random video (This is due to TikTok API). So sometimes after downloading, you may find a completely different video/photo or get data from a completely different post.