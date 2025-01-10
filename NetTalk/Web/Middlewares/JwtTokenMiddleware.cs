using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NetTalk.Middlewares;

public class JwtTokenMiddleware
{
    private readonly RequestDelegate _next;

    public JwtTokenMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value.ToLower();
        var token = context.Session.GetString("AuthToken");
        
        if (string.IsNullOrEmpty(token) && !IsPublicPath(path))
        {
            context.Response.Redirect("/login");
            return;
        }
        
        if (!string.IsNullOrEmpty(token))
        {
            await SetUserFromTokenAsync(context, token);
        }

        await _next(context);
    }
    
    private bool IsPublicPath(string path)
    {
        return path.Contains("/login") || path.Contains("/register");
    }
    
    private async Task SetUserFromTokenAsync(HttpContext context, string token)
    {
        try
        {
            var claimsPrincipal = await GetClaimsPrincipalFromTokenAsync(token);
            if (claimsPrincipal != null)
            {
                context.User = claimsPrincipal;
            }
        }
        catch (Exception ex)
        {
            context.Session.Remove("AuthToken");
        }
    }
    
    private Task<ClaimsPrincipal> GetClaimsPrincipalFromTokenAsync(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
        if (jsonToken == null)
        {
            throw new InvalidOperationException("Invalid token.");
        }

        var claims = jsonToken.Claims;
        var identity = new ClaimsIdentity(claims, "Bearer");
        var principal = new ClaimsPrincipal(identity);

        return Task.FromResult(principal);
    }
}