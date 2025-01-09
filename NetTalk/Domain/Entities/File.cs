using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

[Table("files")]
public class File : BaseEntity
{
    [Column("file_name")]
    public string FileName { get; set; }
    
    [Column("file_type")]
    public FileType Type { get; set; }
    
    [Column("file_size")]
    public long FileSize { get; set; }
    
    [Column("file_url")]
    public string FileUrl { get; set; }
    
    [Column("id_message")]
    public int IdMessage { get; set; }
    
    [Column("id_user")]
    public int IdUser { get; set; }
    
}