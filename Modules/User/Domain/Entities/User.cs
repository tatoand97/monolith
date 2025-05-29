namespace Domain.Entities;

public sealed record class User
{
    public required string Email { get; set; }
    
    public required string Password { get; init; }
    
    public required string Name { get; init; }

    public required string Surname { get; init; }
}