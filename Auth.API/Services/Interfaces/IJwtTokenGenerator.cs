using System.Security.Claims;
using Auth.API.Models;
using Auth.API.Models.SupportTypes;
using Microsoft.IdentityModel.Tokens;

namespace Auth.API.Services.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateFromClaims(IEnumerable<Claim> claims, DateTime expiresAt);
    
    string GenerateToken<T>(T data, DateTime expiresAt);
    
    T? ReadToken<T>(string token, bool shouldThrow = false, TokenValidationParameters? validationParameters = null);
    
    ClaimsPrincipal ReadToken(string token, TokenValidationParameters? validationParameters = null);
    
    TokenValidationParameters CloneParameters();

    RefreshToken GenerateRefreshToken();
    
}