using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Identity;

public class AuthenticationService : IAuthenticationService
{
    private readonly Token _token;
    private readonly IUnitOfWork _unitOfWork;

    public AuthenticationService(
        IOptions<Token> tokenOptions, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _token = tokenOptions.Value;
    }
    

    public async Task<Result<string>> Authenticate(string login, string password)
    {
        var user = GetUserByLogin(login);
        if (user == null)
        {
            return await Result<string>.FailureAsync("Пользователь не найден");
        }

        var isConfirmPassword = VerifyPassword(password, user.Password, user.Salt);
        if (!isConfirmPassword)
        {
            return await Result<string>.FailureAsync("Неверный пароль");
        }
        var jwtToken = await GenerateJwtToken(user);
        return await Result<string>.SuccessAsync(jwtToken);

    }
    


    private bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
    {
        var salt = Convert.FromBase64String(storedSalt);
        using var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, 10000, HashAlgorithmName.SHA256);
        var hash = pbkdf2.GetBytes(32);
        var enteredHash = Convert.ToBase64String(hash);

        return enteredHash == storedHash;
    }

    private User? GetUserByLogin(string login)
    {
        return _unitOfWork.UserRepository.FindByCondition(us => us.Login == login).FirstOrDefault();
    }
    
    private async Task<string> GenerateJwtToken(User user)
    {
        var secret = Encoding.ASCII.GetBytes(_token.Secret);

        var handler = new JwtSecurityTokenHandler();
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Login),
                new Claim("FullName", user.FullName),
                new Claim("PhotoUrl", user.AvatarUrl),
            }),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = handler.CreateToken(descriptor);
        return handler.WriteToken(token);
    }
    
    private (byte[] encryptedSymmetricKey, byte[] iv) GenerateKeyAndEncrypt(string secretKey)
    {
        using var aesAlg = Aes.Create();
        aesAlg.KeySize = 256;
        aesAlg.GenerateKey();
        aesAlg.GenerateIV();

        using var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(Convert.FromBase64String(secretKey), out _);
        var encryptedSymmetricKey = rsa.Encrypt(aesAlg.Key, RSAEncryptionPadding.OaepSHA256);

        return (encryptedSymmetricKey, aesAlg.IV);
    }
    
    private static Role GetRole(int accessLevel)
    {
        if (Enum.IsDefined(typeof(Role), accessLevel))
            return (Role)accessLevel;

        return Role.Default;
    }
}