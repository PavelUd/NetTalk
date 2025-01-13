using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities;

[Table("users")]
public class User : BaseEntity
{
    [Column("login")]
    public string Login { get; set; }
    
    [Column("password")]
    public string Password { get; set; }
    
    [Column("full_name")]
    public string FullName { get; set; }
    
    [Column("salt")]
    public string Salt { get; set; }
    
    [Column("avatar_url")]
    public string AvatarUrl { get; set; }
    
    [Column("is_active")]
    public bool IsActive { get; set; }
    
    [Column("last_online")]
    public DateTime LastOnline{ get; set; }
    
    public SymmetricKey Key{ get; set; }
    public List<Chat> Chats{ get; set; }
}