using Domain.Common;
using MediatR;

namespace Domain.Events.Chat;

public class ChatLastMessageUpdatedEvent (
    Guid id,
    Entities.Message message) : BaseEvent
{
    public Guid IdChat => id;
    public Entities.Message LastMessage { get; } = message;
}