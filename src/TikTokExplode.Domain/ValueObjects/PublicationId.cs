namespace TikTokExplode.Domain.ValueObjects;

public readonly record struct PublicationId(string Value)
{
    /// <summary>
    /// Parses a string into a <see cref="PublicationId"/>.
    /// </summary>
    /// <param name="input">The string to parse.</param>
    /// <returns>A <see cref="PublicationId"/> with the given value.</returns>
    public static PublicationId Parse(string input) => new(input);

    /// <summary>
    /// Implicitly converts a <see cref="PublicationId"/> to its underlying string value.
    /// </summary>
    /// <param name="id">The identifier to convert.</param>
    /// <returns>The string value.</returns>
    public static implicit operator string(PublicationId id) => id.Value;

    /// <summary>
    /// Implicitly converts a string to a <see cref="PublicationId"/>.
    /// </summary>
    /// <param name="value">The string value.</param>
    /// <returns>A <see cref="PublicationId"/> with the given value.</returns>
    public static implicit operator PublicationId(string value) => new(value);

    /// <summary>
    /// Returns the underlying string value.
    /// </summary>
    /// <returns>The string value.</returns>
    public override string ToString() => Value;
}
