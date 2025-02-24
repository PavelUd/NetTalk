using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities;

[Table("crypto_keys")]
public class SymmetricKey : BaseEntity
{
    [Column("id_user")] 
    public Guid  IdUser { get; set; }

    [Column("iv")]
    public byte[] IV{  get; set; }
    
    [Column("crypto_key")]
    public byte[] Key{  get; set; }
    
}