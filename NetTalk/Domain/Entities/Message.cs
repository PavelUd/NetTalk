using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities;

[Table("messages")]
public class Message : BaseAuditableEntity
{
    [Column("id_chat")]
    public int ChatId { get; set; }
    
    [Column("text")]
    public string Text { get; set; }
    public List<File> Files { get; set; }
    public List<MessageStatus> StatusList { get; set; }
}