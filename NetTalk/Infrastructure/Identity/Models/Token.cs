namespace Infrastructure.Identity.Models;

public class Token
{
    public string Secret { get; set; }
    
    public int Expiry { get; set; }
    
    public int RefreshExpiry { get; set; }

}