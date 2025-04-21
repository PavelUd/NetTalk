using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories.Commands;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Identity.Models;
using MediatR;

namespace Application.Commands.Authentication;

public class ConfirmCommand : IRequest<Result<TokenPair>>
{
    public Guid IdRegistration {get; set; }
    public long Code { get; set; }
}

internal class ConfirmCommandHandler(IAuthenticationService service) 
    : IRequestHandler<ConfirmCommand, Result<TokenPair>>
{
    public async Task<Result<TokenPair>> Handle(ConfirmCommand request, CancellationToken cancellationToken)
    {
        try
        {
           var result = await service.ConfirmRegistrationAsync(request.IdRegistration, request.Code);
           return result;
        }
 
        catch (Exception ex)
        {
            return await Result<TokenPair>.FailureAsync(ex.Message);
        }
    }
}