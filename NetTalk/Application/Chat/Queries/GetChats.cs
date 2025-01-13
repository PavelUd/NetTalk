using Application.Chat.Dto;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Application.Chat.Queries;

public record GetChatsQuery : IRequest<Result<List<ChatSummary>>>
{
}


internal class GetChatsQueryQueryHandler : IRequestHandler<GetChatsQuery, Result<List<ChatSummary>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUser _user;

    public GetChatsQueryQueryHandler (IMapper mapper, IUnitOfWork unitOfWork, IUser user)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _user = user;
    }

    public async Task<Result<List<ChatSummary>>> Handle(GetChatsQuery request, CancellationToken cancellationToken)
    {
        var result = _unitOfWork.ChatRepository.FindByCondition(c => c.Users.Any(us => us.Id == _user.Id))
            .ProjectTo<ChatSummary>(_mapper.ConfigurationProvider).ToList();
        return await Result<List<ChatSummary>>.SuccessAsync(result);
    }
}