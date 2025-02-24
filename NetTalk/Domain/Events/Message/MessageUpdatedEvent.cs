namespace Domain.Events.Message;

public class MessageUpdatedEvent(Guid  aggregateId, Guid  idChat, byte[] text, Guid idUser)
    : MessageBaseEvent(aggregateId, idChat, text, idUser);