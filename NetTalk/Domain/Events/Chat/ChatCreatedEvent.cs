namespace Domain.Events.Chat;

public class ChatCreatedEvent(Guid id, string name, string type, bool isActive,List<Guid> participants, Guid  owner)
    : ChatBaseEvent(id, name, type, isActive,participants, owner);