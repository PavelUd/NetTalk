using Domain.Common;

namespace Domain.Events.Message;

public abstract class MessageBaseEvent : BaseEvent
{
    protected MessageBaseEvent(
        int id,
        int aggregateId,
        int idChat,
        byte[] text,
        int idUser)
    {
        Id = id;
        IdChat = idChat;
        Text = text;
        IdUser = idUser;
        AggregateId = aggregateId;
    }

    public int Id { get; private init; }
    public int IdChat { get; set; }
    public byte[] Text { get; set; }
    public int IdUser { get; set; }
}