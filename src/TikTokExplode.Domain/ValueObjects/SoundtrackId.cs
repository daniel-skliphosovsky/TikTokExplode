namespace TikTokExplode.Domain.ValueObjects;

public readonly record struct SoundtrackId(string Value)
{
    /// <summary>
    /// Parses a string into a <see cref="SoundtrackId"/>.
    /// </summary>
    /// <param name="input">The string to parse.</param>
    /// <returns>A <see cref="SoundtrackId"/> with the given value.</returns>
    public static SoundtrackId Parse(string input) => new(input);

    /// <summary>
    /// Implicitly converts a <see cref="SoundtrackId"/> to its underlying string value.
    /// </summary>
    /// <param name="id">The identifier to convert.</param>
    /// <returns>The string value.</returns>
    public static implicit operator string(SoundtrackId id) => id.Value;

    /// <summary>
    /// Implicitly converts a string to a <see cref="SoundtrackId"/>.
    /// </summary>
    /// <param name="value">The string value.</param>
    /// <returns>A <see cref="SoundtrackId"/> with the given value.</returns>
    public static implicit operator SoundtrackId(string value) => new(value);

    /// <summary>
    /// Returns the underlying string value.
    /// </summary>
    /// <returns>The string value.</returns>
    public override string ToString() => Value;
}
