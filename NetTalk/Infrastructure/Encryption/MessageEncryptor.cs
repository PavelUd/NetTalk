using System.Security.Cryptography;
using System.Text;
using Application.Interfaces;

namespace Infrastructure.Encryption;

public class MessageEncryptor(ISymmetricKeyEncryptor symmetricKeyEncryptor) : IMessageEncryptor
{
    private readonly ISymmetricKeyEncryptor _symmetricKeyEncryptor = symmetricKeyEncryptor;

    public byte[] EncryptMessage(byte[] key, byte[] iv, string message)
    {
        using var aesAlg = Aes.Create();
        aesAlg.Key = key;
        aesAlg.IV = iv;
        aesAlg.Mode = CipherMode.CBC;
        aesAlg.Padding = PaddingMode.PKCS7;

        using var encryptor = aesAlg.CreateEncryptor();
        using var msEncrypt = new MemoryStream();
        using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
        
        byte[] messageBytes = Encoding.UTF8.GetBytes(message);
        csEncrypt.Write(messageBytes, 0, messageBytes.Length);
        csEncrypt.FlushFinalBlock(); 

        return msEncrypt.ToArray();
    }
    
    public string DecryptMessage(byte[] encryptedMessage, byte[] symmetricKey, byte[] iv)
    {
        using var aesAlg = Aes.Create();
        aesAlg.Key = symmetricKey;
        aesAlg.IV = iv;

        using var decryptor = aesAlg.CreateDecryptor();
        using var msDecrypt = new MemoryStream(encryptedMessage);
        using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        using var srDecrypt = new StreamReader(csDecrypt);
        return srDecrypt.ReadToEnd();
    }
}