using User.Application.Mappers;

namespace User.Application.Commands.CreateUser;

public class CreateUserHandler(IUnitOfWork unitOfWork)
{
    public async Task Handle(CreateUser command)
    {
        var user = UserMapper.ToEntity(command);
        await unitOfWork.Users.InsertAsync(user);
        await unitOfWork.SaveChangesAsync();
    }
}