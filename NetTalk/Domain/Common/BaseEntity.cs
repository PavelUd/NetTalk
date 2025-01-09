using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common.Interfaces;

namespace Domain.Common;

public class BaseEntity: IEntity
{
    [Column("id")]
    public int Id { get; set; }
}
