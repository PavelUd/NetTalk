using Application.Authentication.Queries;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Authentication.Command;

public record RegisterCommand : IRequest<Result<string>>
{
    public string Login { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
}

internal class  RegisterCommandHandler(IAuthenticationService service, IUnitOfWork unitOfWork)
    : IRequestHandler<RegisterCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (!IsUSerCorrect(request.Login))
            {
                return await Result<string>.FailureAsync("Пользователь с таким логином уже есть");
            }

            var user = new User()
            {
                Login = request.Login,
                Password = request.Password,
                FullName = request.FullName,
                AvtarUrl = "y"
            };
            await unitOfWork.UserRepository.AddAsync(user);
            unitOfWork.Commit();
            var token = await service.Authenticate(user);
            return await Result<string>.SuccessAsync(token);
        }
        catch (Exception e)
        {
            return await Result<string>.FailureAsync(e.Message);
        }
    }

    private bool IsUSerCorrect(string login)
    {
        return !unitOfWork.UserRepository.FindAll().Any(u => u.Login == login);
    }
}
