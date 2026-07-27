namespace TikTokExplode.Domain.Specifications;

public interface IPublicationUrlSpecification
{
    bool IsSatisfiedBy(string? url);
    string GetErrorMessage(string? url);
}
