namespace TikTokExplode.Domain.Exceptions;

public sealed class ValidationException : TikTokExplodeException
{
    public ValidationException(string message) : base(message) { }
    public ValidationException(string message, Exception inner) : base(message, inner) { }
}
