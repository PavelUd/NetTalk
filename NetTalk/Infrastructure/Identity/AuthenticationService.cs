using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces;
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
    

    public AuthenticationService(
        IOptions<Token> tokenOptions){
        
        _token = tokenOptions.Value;
    }
    

    public async Task<string> Authenticate(User user)
    {
        if (user == null) 
            return "";
            
        var jwtToken = await GenerateJwtToken(user);
        return jwtToken;

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
                new Claim("PhotoUrl", user.AvtarUrl),
            }),
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
}