using Domain.Common;

namespace Domain.Events.Chat;

public abstract class ChatBaseEvent : BaseEvent
{

    protected ChatBaseEvent(string name, string type, bool isActive , int owner)
    {
        Name = name;
        Type = type;
        IsActive = isActive;
        Owner = owner;
    }
    
    public string Name { get; private init; }
    
    public string Type { get; private init; }

    public bool IsActive { get; private init; }
    
    public int Owner { get; private init; }
}