using System.ComponentModel.DataAnnotations;

namespace Persistence.Contexts;

public sealed class ConnectionOptions
{
    public static string ConfigSectionPath => "ConnectionStrings";

    [Required]
    public string SqlConnection { get; init; }

    [Required]
    public string NoSqlConnection { get; init; }
    
}