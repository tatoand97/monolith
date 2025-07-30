namespace User.Application.Responses;

public record RegisterResponse
{
    public Guid Id { get; private init; }
    public string Email { get; init; } = string.Empty;

    public static RegisterResponse FromId(Guid id, string email)
    {
        return new RegisterResponse
        {
            Id = id,
            Email = email
        };
    }
}
