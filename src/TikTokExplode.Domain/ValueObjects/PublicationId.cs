namespace TikTokExplode.Domain.ValueObjects;

public readonly record struct PublicationId(string Value)
{
    public static PublicationId Parse(string input) => new(input);
    public static implicit operator string(PublicationId id) => id.Value;
    public static implicit operator PublicationId(string value) => new(value);
    public override string ToString() => Value;
}
