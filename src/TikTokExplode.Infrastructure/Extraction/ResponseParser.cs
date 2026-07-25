using System.Text.Json;
using TikTokExplode.Domain.Exceptions;
using TikTokExplode.Infrastructure.DTOs;

namespace TikTokExplode.Infrastructure.Extraction;

public static class ResponseParser
{
    public static AwemeDto ParseFirstAweme(string jsonResponse)
    {
        var response = JsonSerializer.Deserialize(jsonResponse, TikTokApiJsonContext.Default.TikTokApiResponse);

        return response?.AwemeList?.FirstOrDefault()
            ?? throw new ValidationException("TikTok API returned empty response. The video may not exist or the URL may be invalid.");
    }
}
