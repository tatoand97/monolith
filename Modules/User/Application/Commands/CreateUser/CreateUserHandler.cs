using Common.Domain.Responses;
using User.Application.Mappers;
using User.Application.Responses;
using User.Domain.Interfaces;

namespace User.Application.Commands.CreateUser;

public class CreateUserHandler(IUnitOfWork unitOfWork)
{
    public async Task<Response<RegisterResponse>> Handle(CreateUser command, CancellationToken ct)
    {
        try
        {
            var user = command.ToEntity();
            await unitOfWork.Users.InsertAsync(user, ct);
            await unitOfWork.SaveChangesAsync(ct);

            var response = RegisterResponse.FromId(user.Id, user.Email);
            return Response<RegisterResponse>.Succeed(response, "Usuario registrado exitosamente");
        }
        catch (Exception ex)
        {
            return Response<RegisterResponse>.Fail($"Error al registrar usuario: {ex.Message}");
        }
    }
}