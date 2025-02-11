namespace Domain.Events.Chat;

public class ChatCreatedEvent(string name, string type, bool isActive, int owner)
    : ChatBaseEvent(name, type, isActive, owner);