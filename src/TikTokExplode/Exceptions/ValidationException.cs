namespace TikTokExplode.Exceptions;

/// <summary>
/// Thrown when an argument or data does not pass validation.
/// </summary>
public sealed class ValidationException : TikTokExplodeException
{
    public ValidationException(string message) : base(message) { }

    public ValidationException(string message, Exception inner) : base(message, inner) { }
}
