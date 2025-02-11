namespace Application.Interfaces;

public interface IMessageEncryptor
{
    public byte[] EncryptMessage(byte[] key, byte[] iv, string message);
    public string DecryptMessage(byte[] encryptedMessage, byte[] symmetricKey, byte[] iv);
}