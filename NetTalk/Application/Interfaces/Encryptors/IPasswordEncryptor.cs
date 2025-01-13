namespace Application.Interfaces;

public interface IPasswordEncryptor
{
    public (string hashedPassword, string salt) PasswordEncryption(string password);
}