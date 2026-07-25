namespace TikTokExplode.Domain.Exceptions;

public abstract class TikTokExplodeException : Exception
{
    protected TikTokExplodeException(string message) : base(message) { }
    protected TikTokExplodeException(string message, Exception inner) : base(message, inner) { }
}
