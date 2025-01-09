using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

[Table("chat_invites")]
public class ChatInvite : BaseAuditableEntity
{
    
    [Column("id_chat")]
    public int IdChat { get; set; }
    
    [Column("inviter_user_id")]
    public int InviterUserId { get; set; }
    
    [Column("invitee_user_id")]
    public int inviteeUserId { get; set; }
    
    [Column("status")]
    public InviteStatus Status { get; set; }
    
}