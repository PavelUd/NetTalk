using Application.Chat.Dto;
using Application.Common.Interfaces.Repositories.Query;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Chat;

 public record GetChatByIdQuery : IRequest<Result<ChatSummary>>
 {
        public int Id { get; set; }
 }
 
internal class GetOfficeByIdQueryHandler : IRequestHandler<GetChatByIdQuery, Result<ChatSummary>>
{
    private readonly IMapper _mapper;
    private readonly IChatReadOnlyRepository _repository;

    public GetOfficeByIdQueryHandler (IMapper mapper, IChatReadOnlyRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<Result<ChatSummary>> Handle(GetChatByIdQuery request, CancellationToken cancellationToken)
    {
        var chat = await _repository.GetByIdAsync(request.Id);
        return await Result<ChatSummary>.SuccessAsync(_mapper.Map<ChatSummary>(chat));
    }
}
