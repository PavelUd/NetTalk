using System.ComponentModel.DataAnnotations;
using Application.Commands.Authentication;
using Application.Common.Interfaces;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Authentication.Command;

public record RegisterCommand : IRequest<Result<RegistrationDto>>
{
    [EmailAddress] 
    public string Email { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
}


internal class  RegisterCommandHandler(IAuthenticationService service, IMapper mapper)
    : IRequestHandler<RegisterCommand, Result<RegistrationDto>>
{
    public async Task<Result<RegistrationDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var registration = await service.RegisterAsync(request.Login, request.Password, request.Email);
            if (!registration.Succeeded)
            {
                return await Result<RegistrationDto>.FailureAsync(registration.Errors);
            }
            return await Result<RegistrationDto>.SuccessAsync(mapper.Map<RegistrationDto>(registration.Data));
        }
        
        catch (Exception e)
        {
            return await Result<RegistrationDto>.FailureAsync(e.Message);
        }
    }
}
