using Domain.Common;

namespace Domain.Events.User;

public class UserBaseEvent : BaseEvent
{ 
    public string Username { get; init; }
    
    public string Email { get; init; }
    
    public string Avatar { get; init; }
    
    public List<Guid> PinnedChats { get; init; }

    public UserBaseEvent(Guid id, string username, string email)
    {
        AggregateId = id;
        PinnedChats = new List<Guid>();
        Username = username;
        Email = email;
    }
    
    
}