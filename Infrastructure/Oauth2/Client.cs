namespace Infrastructure.Oauth2;

public class Client
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public required string? Secret { get; set; }

    public required string Name { get; set; }
}