namespace Domain.Events.Message;

public class MessageCreatedEvent(int id,  int idChat, byte[] text, int idUser)
    : MessageBaseEvent(id,id, idChat, text, idUser);