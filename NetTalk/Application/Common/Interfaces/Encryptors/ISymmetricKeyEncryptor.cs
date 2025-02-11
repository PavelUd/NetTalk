namespace Application.Interfaces;

public interface ISymmetricKeyEncryptor
{
    public (byte[] encryptedSymmetricKey, byte[] iv) GenerateKey();
}