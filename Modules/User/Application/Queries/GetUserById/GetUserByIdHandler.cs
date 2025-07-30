using Common.Domain.Responses;
using User.Application.Responses;
using User.Domain.Interfaces;

namespace User.Application.Queries.GetUserById;

public class GetUserByIdHandler(IUnitOfWork unitOfWork)
{
    public async Task<Response<UserResponse>> Handle(GetUserById query, CancellationToken ct)
    {
        var user = await unitOfWork.Users.GetByIdAsync(query.UserId, ct);

        if (user == null)
            return Response<UserResponse>.Fail($"Usuario con ID {query.UserId} no encontrado");

        var response = UserResponse.FromEntity(user, query.UserId);
        return Response<UserResponse>.Succeed(response);
    }
}