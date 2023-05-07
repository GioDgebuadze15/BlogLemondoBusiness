using System.Security.Claims;
using Blog.Services.AppServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Blog.Services;

public static class JwtTokenHelper
{
    public static string GenerateJwtToken(IdentityUser user, IList<Claim>? userClaims = null)
    {
        var handler = new JsonWebTokenHandler();
        var key = new RsaSecurityKey(RsaKey.GetRsaKey());

        // Get the user's claims
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName),
        };

        if (userClaims is not null && userClaims.Any())
            claims.AddRange(userClaims);

        // Create a JWT token
        var token = handler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256),
        });

        return token ?? "empty";
    }
}