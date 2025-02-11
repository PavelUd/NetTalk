using Application.Chat.Dto;
using Application.Common.Interfaces.Repositories.Query;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Chat;

public record GetChatsQuery : IRequest<Result<List<ChatSummary>>>
{
}


internal class GetChatsQueryQueryHandler : IRequestHandler<GetChatsQuery, Result<List<ChatSummary>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUser _user;
    private readonly IChatReadOnlyRepository _readOnlyRepository;

    public GetChatsQueryQueryHandler (IMapper mapper, IUser user, IChatReadOnlyRepository readOnlyRepository)
    {
        _mapper = mapper;
        _user = user;
        _readOnlyRepository = readOnlyRepository;
    }

    public async Task<Result<List<ChatSummary>>> Handle(GetChatsQuery request, CancellationToken cancellationToken)
    {
        var ch = await  _readOnlyRepository.GetAllAsync();
        return await Result<List<ChatSummary>>.SuccessAsync(_mapper.Map<List<ChatSummary>>(ch));
    }
    
}