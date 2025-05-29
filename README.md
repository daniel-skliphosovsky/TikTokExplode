<h1 align="center">
    TikTokExplode
</h1>

<h3 align="center">
    🚀 Powerful .NET library for TikTok API interaction
</h3>

<p align="center">
    <a href="https://github.com/daniel-skliphosovsky/TikTokExplode/releases">
      <img src="https://img.shields.io/badge/Releases-Download-blue?style=for-the-badge&logo=github" alt="Releases">
    </a>
</p>

## Installation

1. **Download DLL**  
   [📥 TikTokExplode.dll](https://github.com/daniel-skliphosovsky/TikTokExplode/releases/download/v1.2.0/TikTokExplode.dll)

2. **Add to project**  
   ```bash
   # .NET CLI
   dotnet add reference path/to/TikTokExplode.dll --project YourProject.csproj
   
   # Visual Studio
   Project → Add → Reference → Browse → select TikTokExplode.dll → Add
   ```
3. **Add namespace**
   ```csharp
   using TikTokExplode;
   ```


## Usage

First, create an instance of TikTokClient:

```csharp
TikTokClient client = new TikTokClient();
```

### Retrieving publications

#### Retrieving publication metadata and components

```csharp
using TikTokExplode;
using TikTokExplode.Publications;
using TikTokExplode.Publications.Videos;
using TikTokExplode.Publications.Images;
using TikTokExplode.Publications.Authors;
using TikTokExplode.Publications.Statistics;
using TikTokExplode.Publications.Soundtracks;

TikTokClient TikTok = new TikTokClient();

Publication publication = await TikTok.Publications.GetAsync("https://publication_url");

Author publicationAuthor = publication.Author;
Stats publicationStatistics = publication.Statistics;
Soundtrack soundtrack = publication.Soundtrack;
Video publicationVideo = publication.Video;
List<Image> publicationImages = publication.Images;

bool publicationIsAds = publication.IsAds;
string publicationid = publication.Id;
string publicationDescription = publication.Description;
```

### Obtaining components separately

#### Retrieving publication author

```csharp
using TikTokExplode;
using TikTokExplode.Publications.Authors;

TikTokClient TikTok = new TikTokClient();

Author author = await TikTok.Publications.Authors.GetAsync("https://publication_url");

bool isVerified = author.IsVerified;
string nickname = author.Nickname;
string region = author.Region;
string thumbAvatarUrl = author.ThumbAvatarUrl;
string mediumAvatarUrl = author.MediumAvatarUrl;
```

#### Retrieving publication statistics 

```csharp
using TikTokExplode;
using TikTokExplode.Publications.Statistics;

TikTokClient TikTok = new TikTokClient();

Stats statistics = await TikTok.Publications.Statistics.GetAsync("https://publication_url");

ulong commentCount = statistics.CommentCount;
ulong playtCount = statistics.PlayCount;
ulong diggCount = statistics.DiggCount;
ulong downloadCount = statistics.DownloadCount;
ulong forwardCount = statistics.ForwardCount;
ulong shareCount = statistics.ShareCount;
ulong repostCount = statistics.RepostCount;
```

#### Retrieving publication soundtrack

```csharp
using TikTokExplode;
using TikTokExplode.Publications.Soundtracks;

TikTokClient TikTok = new TikTokClient();

Soundtrack soundtrack = await TikTok.Publications.Soundtracks.GetAsync("https://publication_url");

ulong id = soundtrack.Id;
string authorNickname = soundtrack.Author;
string title = soundtrack.Title;
string soundUrl = soundtrack.SoundUrl;
string largeCoverUrl = soundtrack.LargeCoverUrl;
string mediumCoverUrl = soundtrack.MediumCoverUrl;
string thumbCoverUrl = soundtrack.ThumbCoverUrl;
```

#### Retrieving publication video

```csharp
using TikTokExplode;
using TikTokExplode.Publications.Videos;

TikTokClient TikTok = new TikTokClient();

Video video = await TikTok.Publications.Videos.GetAsync("https://publication_url");

string videoUrl = video.Url;
int width = video.Width;
int height = video.Height;
ulong duration = video.Duration;
```

#### Retrieving publication images

```csharp
using TikTokExplode;
using TikTokExplode.Publications.Images;

TikTokClient TikTok = new TikTokClient();

List<Image> images = await TikTok.Publications.Images.GetAsync("https://publication_url");

foreach (Image image in images)
{
  string imageUrl = image.Url;
  int width = image.Width;
  int height = image.Height;
}
```

### Downloading publication video

```csharp
using TikTokExplode;
using TikTokExplode.Publications.Videos;

TikTokClient TikTok = new TikTokClient();

Video video = await TikTok.Publications.Videos.GetAsync("https://publication_url");

await TikTok.DownloadVideoAsync(video, "path", "file_name");
// file_name option is not required. If you not set it file will be named publication_id
```

### Downloading publication images

```csharp
using TikTokExplode;
using TikTokExplode.Publications.Images;

TikTokClient TikTok = new TikTokClient();

List<Image> images = await TikTok.Publications.Images.GetAsync("https://publication_url");

await TikTok.DownloadImagesAsync(images, "path", $"file_name");
// file_name option is not required. If you not set it file will be named publication_id

```
### Working with TikTokExplode exceptions
```csharp
using TikTokExplode.Exceptions;

try
{
    //some code
}
catch(TikTokExplodeException)
{
    //triggered when an exception is thrown by TikTokExplode
}
catch(Exception)
{
    //other exceptions
}
```

### Downloading using Progress and CancellationToken
```csharp

// Example with Progress

using TikTokExplode;
using TikTokExplode.Publications.Videos;

TikTokClient TikTok = new TikTokClient();

Video video = await TikTok.Publications.Videos.GetAsync("https://publication_url");

Progress<double> progress = new Progress<double>(procent =>
{
    //triggered when progress value changed
});

await TikTok.DownloadVideoAsync(video, "path", progress: progress);

```
```csharp

// Example with CancellationToken

using TikTokExplode;
using TikTokExplode.Publications.Videos;

TikTokClient TikTok = new TikTokClient();

Video video = await TikTok.Publications.Videos.GetAsync("https://publication_url");

using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

// for cancel downloading -> cancellationTokenSource.Cancel();

try
{
    await TikTok.DownloadVideoAsync(video, "path", cancellationToken: cancellationTokenSource.Token);
}
catch(OperationCanceledException)
{
    //triggered when downloading has been canceled
}
```

### Getting type of TikTokPublication by link
```csharp
using TikTokExplode;
using TikTokExplode.Publications;
using TikTokExplode.Publications.Videos;
using TikTokExplode.Publications.Images;

TikTokClient TikTok = new TikTokClient();

string url = "https://publication_url";

PublicationClient.PublicationType publicationType = await PublicationClient.GetPublicationType(url);

switch (publicationType)
{
    case PublicationClient.PublicationType.Images:
        //...
        List<Image> images = await TikTok.Publications.Images.GetAsync(url);
        //...
        break;

    case PublicationClient.PublicationType.Video:
        //...
        Video video = await TikTok.Publications.Videos.GetAsync(url);
        //...
        break;

    case PublicationClient.PublicationType.Unknown:
    default:
        // Link is incorrect
        break;
}
```




