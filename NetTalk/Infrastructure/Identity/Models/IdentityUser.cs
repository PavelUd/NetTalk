using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Identity.Models;

public class IdentityUser : IUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdentityUser()
    {
        
    }
    public IdentityUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public Guid  Id
    {
        get
        {
            var value = _httpContextAccessor.HttpContext?.User.FindFirst("Id")?.Value;
            if (value != null)
                return Guid .Parse(value);
            return Guid.Empty;
        }
        init { }
    }

    public string Name => _httpContextAccessor.HttpContext?.User.Identity?.Name ?? string.Empty;

    public string AvatarUrl
    {
        get
        {
            var value = _httpContextAccessor.HttpContext?.User.FindFirst("PhotoUrl")?.Value;
            if (value != null)
                return value;
            return "";
        }
        init { }
    }

}