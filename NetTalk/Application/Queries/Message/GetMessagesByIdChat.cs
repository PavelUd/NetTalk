using Application.Common.Interfaces.Repositories.Query;
using Application.Queries.QueryModels;
using MediatR;

namespace Application.Queries.Message;

public record GetMessagesByIdChat : IRequest<List<MessageQueryModel>>
{
    public Guid IdChat { get; init; }
}

internal class GetChatsQueryQueryHandler : IRequestHandler<GetMessagesByIdChat, List<MessageQueryModel>>
{
    private readonly IMessageReadOnlyRepository _messagesRepository;

    public GetChatsQueryQueryHandler(IMessageReadOnlyRepository messagesRepository)
    {
        _messagesRepository = messagesRepository;
    }
    
    public async Task<List<MessageQueryModel>> Handle(GetMessagesByIdChat request, CancellationToken cancellationToken)
    {
       var messages = await _messagesRepository.GetChatMessages(request.IdChat);
       return messages.ToList();
    }
}