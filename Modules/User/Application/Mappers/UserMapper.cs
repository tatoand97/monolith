using User.Application.Commands.CreateUser;
using User.Application.DTOs;
using User.Domain.Entities;

namespace User.Application.Mappers;

public static class UserMapper
{
    private static UserDto ToDto(this UserEntity entity)
    {
        return new UserDto
        {
            Id = entity.Id,
            Email = entity.Email,
            Name = entity.Name,
            Surname = entity.Surname
        };
    }

    public static IReadOnlyList<UserDto> ToDtoList(this IReadOnlyList<UserEntity> entities)
    {
        return entities.Select(e => e.ToDto()).ToList();
    }

    public static CreateUser ToCreateDto(this UserEntity entity)
    {
        return new CreateUser(
            entity.Email,
            entity.Password,
            entity.Name,
            entity.Surname
        );
    }

    public static UserEntity ToEntity(this CreateUser command)
    {
        return new UserEntity
        {
            Email = command.Email,
            Password = command.Password,
            Name = command.Name,
            Surname = command.Surname
        };
    }
}