using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

[Table("message_statuses")]
public class MessageStatus : BaseEntity
{
    [Column("id_message")] public int IdMessage { get; set; }

    [Column("id_user")] public int IdUser { get; set; }

    [Column("status")] public MsgStatus Status { get; set; }
}
