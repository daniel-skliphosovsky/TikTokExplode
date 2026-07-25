namespace TikTokExplode.Domain.ValueObjects;

public readonly record struct AuthorId(string Value)
{
    public static AuthorId Parse(string input) => new(input);
    public static implicit operator string(AuthorId id) => id.Value;
    public static implicit operator AuthorId(string value) => new(value);
    public override string ToString() => Value;
}
