using System.Text.Json;
using TikTokExplode.Domain.Entities;
using TikTokExplode.Domain.Exceptions;
using TikTokExplode.Domain.ValueObjects;

namespace TikTokExplode.Infrastructure.Extraction;

public sealed class SoundtrackExtractor : ISoundtrackExtractor
{
    public Soundtrack ExtractSoundtrack(string jsonResponse)
    {
        try
        {
            using var doc = JsonDocument.Parse(jsonResponse);
            var awemeList = doc.RootElement.GetProperty("aweme_list");
            var musicData = awemeList[0].GetProperty("music");

            var soundtrack = new Soundtrack
            {
                Id = SoundtrackId.Parse(musicData.GetProperty("id").GetString() ?? string.Empty),
                Title = musicData.GetProperty("title").GetString() ?? string.Empty,
                AuthorName = musicData.GetProperty("author").GetString() ?? string.Empty,
                SoundUrl = musicData.GetProperty("play_url").GetProperty("url_list")[0].GetString() ?? string.Empty,
                LargeCoverUrl = musicData.GetProperty("cover_large").GetProperty("url_list")[0].GetString() ?? string.Empty,
                MediumCoverUrl = musicData.GetProperty("cover_medium").GetProperty("url_list")[0].GetString() ?? string.Empty,
                ThumbCoverUrl = musicData.GetProperty("cover_thumb").GetProperty("url_list")[0].GetString() ?? string.Empty
            };

            return soundtrack;
        }
        catch (JsonException ex)
        {
            throw new ApiException("Failed to parse soundtrack JSON", 0, jsonResponse, ex);
        }
    }
}
