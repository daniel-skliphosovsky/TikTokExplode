namespace TikTokExplode.Exceptions;

/// <summary>
/// Thrown when the TikTok API or a media server returns an error response.
/// </summary>
public sealed class ApiException : TikTokExplodeException
{
    /// <summary>
    /// HTTP status code returned by the server.
    /// </summary>
    public int StatusCode { get; }

    /// <summary>
    /// Raw response body returned by the server.
    /// </summary>
    public string ResponseBody { get; }

    public ApiException(string message, int statusCode, string responseBody)
        : base(message)
    {
        StatusCode = statusCode;
        ResponseBody = responseBody;
    }

    public ApiException(string message, int statusCode, string responseBody, Exception inner)
        : base(message, inner)
    {
        StatusCode = statusCode;
        ResponseBody = responseBody;
    }
}
