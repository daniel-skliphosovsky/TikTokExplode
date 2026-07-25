using System.Text.Json;
using TikTokExplode.Domain.Entities;
using TikTokExplode.Domain.Exceptions;

namespace TikTokExplode.Infrastructure.Extraction;

public sealed class ImageExtractor : IImageExtractor
{
    public IReadOnlyList<Image> ExtractImages(string jsonResponse)
    {
        try
        {
            using var doc = JsonDocument.Parse(jsonResponse);
            var awemeList = doc.RootElement.GetProperty("aweme_list");
            
            if (awemeList.GetArrayLength() == 0)
                throw new ValidationException("No image data in response");

            var images = new List<Image>();
            var imageData = awemeList[0].GetProperty("image_post_info");

            foreach (var img in imageData.GetProperty("images").EnumerateArray())
            {
                images.Add(new Image
                {
                    AwemeId = awemeList[0].GetProperty("aweme_id").GetString() ?? string.Empty,
                    ImageUrl = img.GetProperty("display_image").GetProperty("url_list")[0].GetString() ?? string.Empty,
                    Width = img.GetProperty("display_image").GetProperty("width").GetInt32(),
                    Height = img.GetProperty("display_image").GetProperty("height").GetInt32()
                });
            }

            return images.AsReadOnly();
        }
        catch (JsonException ex)
        {
            throw new ApiException("Failed to parse image JSON", 0, jsonResponse, ex);
        }
    }
}
