using Application.Commands.Chat.Dto;
using Application.Common.Interfaces.Repositories.Query;
using Application.Common.Result;
using Application.Interfaces;
using MediatR;

namespace Application.Queries.Chat;

public record GetDirectIdByUserId : IRequest<Result<Guid?>>
{
    public Guid IdOtherUser { get; init; }
}

internal class GetDirectByUserIdQueryHandler : IRequestHandler<GetDirectIdByUserId, Result<Guid?>>
{
    private IChatReadOnlyRepository _repository;
    private IUser user;

    public GetDirectByUserIdQueryHandler(IChatReadOnlyRepository repository, IUser user)
    {
        _repository = repository;
        this.user = user;
    }

    public async Task<Result<Guid?>> Handle(GetDirectIdByUserId request, CancellationToken cancellationToken)
    {
        var chat = await _repository.GetAllAsync();
        var id = chat.FirstOrDefault(x =>
            x.Participants.Count == 2 &&
            x.Participants.Contains(request.IdOtherUser) &&
            x.Participants.Contains(user.Id))?.Id;
        return await Result<Guid?>.SuccessAsync(id);
    }
}