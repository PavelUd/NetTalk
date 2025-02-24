using Domain.Common;

namespace Domain.Events.Message;

public abstract class MessageBaseEvent : BaseEvent
{
    protected MessageBaseEvent(
        Guid  aggregateId,
        Guid idChat,
        byte[] text,
        Guid  idUser)
    {
        IdChat = idChat;
        Text = text;
        IdUser = idUser;
        AggregateId = aggregateId;
    }
    
    public Guid IdChat { get; set; }
    public byte[] Text { get; set; }
    public Guid  IdUser { get; set; }
}