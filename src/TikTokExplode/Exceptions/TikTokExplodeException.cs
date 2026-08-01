namespace TikTokExplode.Exceptions;

/// <summary>
/// Base exception for all errors thrown by TikTokExplode.
/// </summary>
public abstract class TikTokExplodeException : Exception
{
    protected TikTokExplodeException(string message) : base(message) { }

    protected TikTokExplodeException(string message, Exception inner) : base(message, inner) { }
}
