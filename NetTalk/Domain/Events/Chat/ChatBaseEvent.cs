using Domain.Common;

namespace Domain.Events.Chat;

public abstract class ChatBaseEvent : BaseEvent
{

    protected ChatBaseEvent(Guid id, string name, string type, bool isActive ,List<Guid> participants, Guid  owner)
    {
        Name = name;
        Type = type;
        IsActive = isActive;
        Participants = participants;
        Owner = owner;
       AggregateId = id;
    }
    
    public string Name { get; private init; }
    
    public string Type { get; private init; }

    public bool IsActive { get; private init; }
    
    public List<Guid> Participants { get; }
    
    public Guid  Owner { get; private init; }
}