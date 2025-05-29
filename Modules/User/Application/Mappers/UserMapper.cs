using Application.Commands.CreateUser;
using Domain.Entities;

namespace Application.Mappers;

public static class UserMapper
{
    public static CreateUser ToCreateDto(User user) => new(user.Email, user.Password, user.Name, user.Surname);
    
    public static User ToEntity(CreateUser user) => new()
    {
        Email = user.Email,
        Password = user.Password,
        Name = user.Name,
        Surname = user.Surname
    };
}