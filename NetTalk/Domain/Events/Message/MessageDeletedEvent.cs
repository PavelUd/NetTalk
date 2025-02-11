namespace Domain.Events.Message;

public class MessageDeletedEvent(int id, int aggregateId, int idChat, byte[] text, int idUser)
    : MessageBaseEvent(id, aggregateId, idChat, text, idUser);
