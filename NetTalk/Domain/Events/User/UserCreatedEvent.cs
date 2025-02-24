namespace Domain.Events.User;

public class UserCreatedEvent(Guid id, string username, string email) : UserBaseEvent(id, username, email);