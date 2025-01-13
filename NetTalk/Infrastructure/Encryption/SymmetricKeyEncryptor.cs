using System.Security.Cryptography;
using System.Text;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace Infrastructure.Encryption;

public class SymmetricKeyEncryptor : ISymmetricKeyEncryptor
{

    private readonly string _secretKey;
    public SymmetricKeyEncryptor(IConfiguration configuration)
    {
        _secretKey = configuration["secret_token"];
    }
    
    public (byte[] encryptedSymmetricKey, byte[] iv) GenerateKey()
    {
        using var aesAlg = Aes.Create();
        aesAlg.KeySize = 256;
        aesAlg.GenerateKey();

        return (aesAlg.Key, aesAlg.IV);
        
    }
}