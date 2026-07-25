namespace TikTokExplode.Domain.ValueObjects;

public readonly record struct AuthorId(string Value)
{
    /// <summary>
    /// Parses a string into an <see cref="AuthorId"/>.
    /// </summary>
    /// <param name="input">The string to parse.</param>
    /// <returns>An <see cref="AuthorId"/> with the given value.</returns>
    public static AuthorId Parse(string input) => new(input);

    /// <summary>
    /// Implicitly converts an <see cref="AuthorId"/> to its underlying string value.
    /// </summary>
    /// <param name="id">The identifier to convert.</param>
    /// <returns>The string value.</returns>
    public static implicit operator string(AuthorId id) => id.Value;

    /// <summary>
    /// Implicitly converts a string to an <see cref="AuthorId"/>.
    /// </summary>
    /// <param name="value">The string value.</param>
    /// <returns>An <see cref="AuthorId"/> with the given value.</returns>
    public static implicit operator AuthorId(string value) => new(value);

    /// <summary>
    /// Returns the underlying string value.
    /// </summary>
    /// <returns>The string value.</returns>
    public override string ToString() => Value;
}
