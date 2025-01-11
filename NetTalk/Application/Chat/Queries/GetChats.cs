using Application.Chat.Dto;
using Application.Common.Result;
using Application.Interfaces.Repositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Application.Chat.Queries;

public record GetChatsQuery : IRequest<Result<List<ChatDto>>>
{
    public int Id { get; set; }
}


internal class GetChatsQueryQueryHandler : IRequestHandler<GetChatsQuery, Result<List<ChatDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetChatsQueryQueryHandler (IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<ChatDto>>> Handle(GetChatsQuery request, CancellationToken cancellationToken)
    {
        var result = _unitOfWork.ChatRepository.FindAll()
            .ProjectTo<ChatDto>(_mapper.ConfigurationProvider).ToList();
        return await Result<List<ChatDto>>.SuccessAsync(result);
    }
}