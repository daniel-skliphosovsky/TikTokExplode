namespace TikTokExplode.Domain.ValueObjects;

public readonly record struct SoundtrackId(string Value)
{
    public static SoundtrackId Parse(string input) => new(input);
    public static implicit operator string(SoundtrackId id) => id.Value;
    public static implicit operator SoundtrackId(string value) => new(value);
    public override string ToString() => Value;
}
