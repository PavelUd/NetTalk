namespace Domain.Events.Message;

public class MessageUpdatedEvent(int id, int aggregateId, int idChat, byte[] text, int idUser)
    : MessageBaseEvent(id, aggregateId, idChat, text, idUser);