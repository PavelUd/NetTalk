using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common.Interfaces;

namespace Domain.Common;

public class BaseAuditableEntity : BaseEntity, IAuditableEntity
{
        
    [Column("created_at")]
    public DateTime? CreatedDate { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedDate { get; set; }
}