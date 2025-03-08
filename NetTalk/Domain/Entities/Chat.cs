using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Events.Chat;

namespace Domain.Entities;

[Table("chats")]

public class Chat : BaseAuditableEntity
{
    public Chat()
    {

    }

    public Chat(string name, string type, bool isActive, Guid owner, List<User> users)
    {
        Name = name;
        Type = type;
        IsActive = isActive;
        Owner = owner;
        Users = users;
        UpdatedDate = DateTime.Now.ToUniversalTime();
        CreatedDate = DateTime.Now.ToUniversalTime();

        AddDomainEvent(new ChatCreatedEvent(Id, name, type, isActive, users.Select(user => user.Id).ToList(), owner));
    }
    
    [Column("chat_name")]
    public string Name { get; init; }
    
    [Column("chat_type")]
    public string Type { get; set; }
    
    [Column("is_active")] 
    public bool IsActive { get; set; }
    
    [Column("created_by")]
    public Guid  Owner { get; set; }
    
    public List<Message> Messages { get; set; }
    public List<User> Users { get; set; }
}