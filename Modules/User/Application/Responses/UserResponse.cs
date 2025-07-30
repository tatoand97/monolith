using User.Domain.Entities;

namespace User.Application.Responses;

public record UserResponse
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Surname { get; init; } = string.Empty;

    public static UserResponse FromEntity(UserEntity entity, Guid id)
    {
        return new UserResponse
        {
            Id = id,
            Email = entity.Email,
            Name = entity.Name,
            Surname = entity.Surname
        };
    }
}
