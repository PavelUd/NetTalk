namespace Domain.Events.Chat;

public class ChatCreatedEvent(string name, string type, bool isActive,List<Guid> participants, Guid  owner)
    : ChatBaseEvent(name, type, isActive,participants, owner);