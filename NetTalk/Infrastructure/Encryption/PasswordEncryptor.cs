using System.Security.Cryptography;
using Application.Interfaces;

namespace Infrastructure.Encryption;

public class PasswordEncryptor : IPasswordEncryptor
{
    public  (string hashedPassword, string salt) PasswordEncryption(string password)
    {
            var salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
            var hash = pbkdf2.GetBytes(32);
            var hashedPassword = Convert.ToBase64String(hash);
            var saltBase64 = Convert.ToBase64String(salt);

            return (hashedPassword, saltBase64);
    }
}