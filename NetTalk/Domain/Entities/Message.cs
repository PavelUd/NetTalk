using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities;

[Table("messages")]
public class Message : BaseAuditableEntity
{
    [Column("id_chat")]
    public int IdChat { get; set; }
    
    [Column("message")]
    public byte[] Text { get; set; }    
    
    [Column("id_user")]
    public int IdUser { get; set; }
    public List<File> Files { get; set; }
    public List<MessageStatus> StatusList { get; set; }
}