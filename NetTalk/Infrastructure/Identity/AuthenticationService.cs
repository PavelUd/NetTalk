using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories.Commands;
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
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IHashingService _hashingService;
    private readonly IEmailService _emailService;
    private readonly ISymmetricKeyEncryptor _encryptor;
    private readonly ICacheService _cacheService;
    private readonly IUnitOfWork _unitOfWork;
    

    public AuthenticationService(IOptions<Token> tokenOptions, 
        ISymmetricKeyEncryptor encryptor,
        IUserRepository userRepository, 
        IRefreshTokenRepository refreshTokenRepository, 
        IHashingService hashingService, 
        IUnitOfWork unitOfWork, ICacheService cacheService, IEmailService emailService)
    {
        _encryptor = encryptor;
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _hashingService = hashingService;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
        _emailService = emailService;
        _token = tokenOptions.Value;
    }
    

    public async Task<Result<TokenPair>> Authenticate(string login, string password)
    {
        var user = GetUserByLogin(login);
        if (user == null)
        {
            return await Result<TokenPair>.FailureAsync("Пользователь не найден");
        }

        var isConfirmPassword = VerifyPassword(password, user.Password, user.Salt);
        if (!isConfirmPassword)
        {
            return await Result<TokenPair>.FailureAsync("Неверный пароль");
        }
        var tokens = await GenerateTokenPairAsync(user);
        
        return await Result<TokenPair>.SuccessAsync(tokens);

    }

    public async Task SendPasswordResetLinkAsync(string email)
    {
        var user = GetUserByEmail(email);
        if (user == null)
        {
            return;
        }

        var token= await GenerateJwtToken(user, new TimeSpan(0, 15, 0));
        _emailService.SendEmail(token, email);
        
    }

    public async Task<Result<string>> ResetPasswordAsync(string token, string newPassword)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
        var info = jsonToken?.Claims.FirstOrDefault(c => c.Type == "FullName")?.Value;

        if (info == null)
        {
            return await Result<string>.FailureAsync("Неверный токен");
        }
        if (jsonToken.ValidTo < DateTime.UtcNow)
        {
            return await Result<string>.FailureAsync("Токен истек");
        }
        var user = GetUserByEmail(info);
        if (user == null)
        {
            return await Result<string>.FailureAsync("Пользователь не найден");
        }

        await ResetPassword(user, newPassword);
        return await Result<string>.SuccessAsync("OK");
    }

    public async Task<Result<TokenPair>> Authenticate(string refreshToken)
    {
        var hashToken = _hashingService.Hash(refreshToken);
        var storedToken = _refreshTokenRepository.FindByCondition(x => x.Token == hashToken).FirstOrDefault();
        if (storedToken == null)
        {
            return await Result<TokenPair>.FailureAsync("Токен не найден");
        }

        var user = _userRepository.FindByCondition(us => us.Id == storedToken.UserId).First();
        var tokens = await GenerateTokenPairAsync(user);
        
        return await Result<TokenPair>.SuccessAsync(tokens);

    }

   public async Task<TokenPair> GenerateTokenPairAsync(User user)
    {
        var accessToken = await GenerateJwtToken(user, new TimeSpan(4, 0, 0));
        var refreshToken = GenerateOpaqueToken();
        
        await SaveRefreshToken(refreshToken, user.Id);
        
        return new TokenPair(accessToken, refreshToken);
    }

    public async Task<Result<TokenPair>> ConfirmRegistrationAsync(Guid registrationId, long code)
    {
        var registration = _cacheService.Get<Registration>(registrationId);
        if (registration == null)
        {
            return await Result<TokenPair>.FailureAsync($"Регистарция с id {registrationId} не найдена");
        }

        if (registration.Code != code)
        {
            return await Result<TokenPair>.FailureAsync("Неверный код");
        }
            
        var key = _encryptor.GenerateKey();
        var user = new User(registration.Login, registration.Password, registration.Email, registration.Salt, "", key);
            
        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        var tokenPair = await GenerateTokenPairAsync(user);
        return await Result<TokenPair>.SuccessAsync(tokenPair);
    }

    public async Task<Result<Registration>> RegisterAsync(string login, string password, string email)
    {
        if (!IsUniqueLogin(login))
        {
            return await Result<Registration>.FailureAsync("Такой логин уже есть");
        }

        if (!IsUniqueEmail(email))
        {
            return await Result<Registration>.FailureAsync($"Аккаут с емейлом {email} уже существует");
        }

        var code = GenerateVerificationCode();
        var (passwordHash, salt) = _hashingService.HashWithSalt(password);
        var registration = new Registration(login,passwordHash, email,  salt, code);
            
        _cacheService.Set(registration.Id, registration);
        _emailService.SendEmail(code.ToString(), email);

        return await Result<Registration>.SuccessAsync(registration);
    }

    private async Task ResetPassword(User user, string newPassword)
    {
        var (newHashingPassword, newSalt) = _hashingService.HashWithSalt(newPassword);
        user.Password = newHashingPassword;
        user.Salt = newSalt;
        await _userRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }
    private static string GenerateOpaqueToken()
    {
        var randomBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        var token = Convert.ToHexString(randomBytes);
        return token;
    }

    private async Task SaveRefreshToken(string token, Guid userId)
    {
        var hashToken = _hashingService.Hash(token);
        var oldRefreshToken = _refreshTokenRepository.FindByCondition(x => x.UserId == userId).FirstOrDefault();

        if (oldRefreshToken != null)
        {
            oldRefreshToken.Token = hashToken;
            oldRefreshToken.UpdatedDate = DateTime.Now.ToUniversalTime();
            await  _refreshTokenRepository.UpdateAsync(oldRefreshToken);
            return;
        }
        
        var refreshToken = new RefreshToken(hashToken, userId);
        await  _refreshTokenRepository.AddAsync(refreshToken);
        await _unitOfWork.SaveChangesAsync();
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
        return  _userRepository.FindByCondition(us => us.Login == login).FirstOrDefault();
    }
    
    private User? GetUserByEmail(string mail)
    {
        return  _userRepository.FindByCondition(us => us.FullName == mail).FirstOrDefault();
    }
    
    private async Task<string> GenerateJwtToken(User user, TimeSpan lifetime)
    {
        var secret = Encoding.ASCII.GetBytes(_token.Secret);

        var handler = new JwtSecurityTokenHandler();
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new ("Id", user.Id.ToString()),
                new (ClaimTypes.Name, user.Login),
                new ("FullName", user.FullName),
            }),
            Expires = DateTime.UtcNow.Add(lifetime),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = handler.CreateToken(descriptor);
        return handler.WriteToken(token);
    }
    
    private static Role GetRole(int accessLevel)
    {
        if (Enum.IsDefined(typeof(Role), accessLevel))
            return (Role)accessLevel;

        return Role.Default;
    }
    
    private bool IsUniqueLogin(string login)
    {
        return !_userRepository.FindAll().Any(u => u.Login == login);
    }

    private bool IsUniqueEmail(string email)
    {
        return !_userRepository.FindAll().Any(u => u.FullName == email);
    }
    
    private long GenerateVerificationCode()
    {
        var _random = new Random();
        var code = _random.Next(100000, 1000000);
        return code;
    }
    
    
}