namespace User.Domain.Entities;

public sealed record UserEntity
{
    public required string Email { get; set; }

    public required string Password { get; init; }

    public required string Name { get; init; }

    public required string Surname { get; init; }
}