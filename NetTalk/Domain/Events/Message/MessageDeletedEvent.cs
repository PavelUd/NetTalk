namespace Domain.Events.Message;

public class MessageDeletedEvent(Guid  aggregateId, Guid idChat, byte[] text,Guid  idUser)
    : MessageBaseEvent(aggregateId, idChat, text, idUser);
