using Domain.Common;

namespace Domain.Entities;

public class Registration(string login, string password, string email, string salt, long code)
    : BaseEntity
{
    public string Login { get; set; } = login;
    public string Password { get; set; } = password;
    public string Email { get; set; } = email;
    public string Salt { get; set; } = salt;
    public long Code { get; set; } = code;
}