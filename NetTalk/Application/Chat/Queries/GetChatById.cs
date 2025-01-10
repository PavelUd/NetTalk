using Application.Chat.Dto;
using Application.Common.Result;
using Application.Interfaces.Repositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Application.Chat.Queries;

 public record GetChatByIdQuery : IRequest<Result<ChatDto>>
 {
        public int Id { get; set; }
 }
 
internal class GetOfficeByIdQueryHandler : IRequestHandler<GetChatByIdQuery, Result<ChatDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetOfficeByIdQueryHandler (IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ChatDto>> Handle(GetChatByIdQuery request, CancellationToken cancellationToken)
    {
        var chat = _unitOfWork.ChatRepository
            .FindByCondition(c => c.Id == request.Id)
            .ProjectTo<ChatDto>(_mapper.ConfigurationProvider)
            .FirstOrDefault();
        
        return await Result<ChatDto>.SuccessAsync(chat);
    }
}
