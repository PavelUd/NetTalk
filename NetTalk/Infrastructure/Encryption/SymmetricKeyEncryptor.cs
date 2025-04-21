using System.Security.Cryptography;
using System.Text;
using Application.Interfaces;
using Domain.Entities;
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

    public SymmetricKeyEncryptor()
    {
        
    }
    public SymmetricKey GenerateKey()
    {
        using var aesAlg = Aes.Create();
        aesAlg.KeySize = 256;
        aesAlg.GenerateKey();

        return new SymmetricKey() {
            Key = aesAlg.Key, 
            IV = aesAlg.IV
        };
        
    }
}