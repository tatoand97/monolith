using User.Application.Commands.CreateUser;

namespace User.Application.Mappers;

public static class UserMapper
{
    public static CreateUser ToCreateDto(Domain.Entities.UserEntity userEntity) => new(userEntity.Email, userEntity.Password, userEntity.Name, userEntity.Surname);
    
    public static Domain.Entities.UserEntity ToEntity(CreateUser user) => new()
    {
        Email = user.Email,
        Password = user.Password,
        Name = user.Name,
        Surname = user.Surname
    };
}