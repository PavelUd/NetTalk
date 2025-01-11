using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

[Table("chats")]

public class Chat : BaseAuditableEntity
{
    [Column("chat_name")]
    public string Name { get; set; }
    
    [Column("chat_type")]
    public string Type { get; set; }
    
    [Column("is_active")]
    public bool IsActive { get; set; }
    
    [Column("created_by")]
    public int Owner { get; set; }

    public List<Message> Messages { get; set; }
    public List<User> Users { get; set; }
}