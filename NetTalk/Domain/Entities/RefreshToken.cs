using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities;

[Table("refresh_tokens")]
public class RefreshToken : BaseAuditableEntity
{
    [Column("token")]
    public string Token { get; set; }
    
    [Column("user_id")]
    public Guid UserId { get; set; }
    

    public RefreshToken(string token, Guid userId)
    {
        UserId = userId;
        Token = token;
    }
}