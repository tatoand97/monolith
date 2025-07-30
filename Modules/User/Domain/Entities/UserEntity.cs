namespace User.Domain.Entities;

public sealed record UserEntity
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public required string Email { get; init; }

    public required string Password { get; init; }

    public required string Name { get; init; }

    public required string Surname { get; init; }
}