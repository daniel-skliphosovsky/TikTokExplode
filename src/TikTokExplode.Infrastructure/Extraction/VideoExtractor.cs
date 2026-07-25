using System.Text.Json;
using TikTokExplode.Domain.Entities;
using TikTokExplode.Domain.Exceptions;

namespace TikTokExplode.Infrastructure.Extraction;

public sealed class VideoExtractor : IVideoExtractor
{
    public Video ExtractVideo(string jsonResponse)
    {
        try
        {
            using var doc = JsonDocument.Parse(jsonResponse);
            var awemeList = doc.RootElement.GetProperty("aweme_list");
            
            if (awemeList.GetArrayLength() == 0)
                throw new ValidationException("No video data in response");

            var videoData = awemeList[0].GetProperty("video");

            var video = new Video
            {
                AwemeId = awemeList[0].GetProperty("aweme_id").GetString() ?? string.Empty,
                PlayUrl = videoData.GetProperty("play_addr").GetProperty("url_list")[0].GetString() ?? string.Empty,
                Width = videoData.GetProperty("width").GetInt32(),
                Height = videoData.GetProperty("height").GetInt32(),
                Duration = videoData.GetProperty("duration").GetInt32()
            };

            return video;
        }
        catch (JsonException ex)
        {
            throw new ApiException("Failed to parse video JSON", 0, jsonResponse, ex);
        }
    }
}
