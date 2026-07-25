namespace TikTokExplode.Domain.Exceptions;

public sealed class ApiException : TikTokExplodeException
{
    public int StatusCode { get; }
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
