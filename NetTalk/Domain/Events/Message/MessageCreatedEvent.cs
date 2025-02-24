namespace Domain.Events.Message;

public class MessageCreatedEvent(Guid  id,  Guid  idChat, byte[] text, Guid  idUser)
    : MessageBaseEvent(id, idChat, text, idUser);