using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Auth.API.Models;
using Auth.API.Services.Interfaces;
using Auth.API.Services.SupportTypes;
using Microsoft.IdentityModel.Tokens;

namespace Auth.API.Services;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IConfiguration _configuration;
    private readonly TokenValidationParameters _defaultValidationParameters;
    
    public JwtTokenGenerator(IHttpContextAccessor contextAccessor, IConfiguration configuration)
    {
        _contextAccessor = contextAccessor;
        _configuration = configuration;
        _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        _defaultValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration.GetValue<string>("Jwt:Key")!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    }

    public string GenerateFromClaims(IEnumerable<Claim> claims, DateTime expiresAt)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims.ToArray()),
            Expires = expiresAt,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(_configuration.GetValue<string>("Jwt:Key")!)),
                    SecurityAlgorithms.HmacSha256Signature)
        };
        var token = _jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
        return _jwtSecurityTokenHandler.WriteToken(token);
    }

    public string GenerateToken<T>(T data, DateTime expiresAt)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(typeof(T).Name, JsonSerializer.Serialize(data))
            }),
            Expires = expiresAt,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration.GetValue<string>("Jwt:Key")!)), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = _jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
        return _jwtSecurityTokenHandler.WriteToken(token);
    }

    public T? ReadToken<T>(string token, bool shouldThrow = false, TokenValidationParameters? validationParameters = null)
    {
        try
        {
            var principal = ReadToken(token, validationParameters);
            var claim = principal.Claims.FirstOrDefault(c => c.Type == typeof(T).Name);
            return JsonSerializer.Deserialize<T>(claim.Value);    
        }catch (Exception e)
        {
            if (shouldThrow)
            {
                throw;
            }
            return default;
        }
    }
    
    public ClaimsPrincipal ReadToken(string token, TokenValidationParameters? validationParameters = null) 
        => _jwtSecurityTokenHandler.ValidateToken(
            token, 
            validationParameters ?? _defaultValidationParameters, 
            out _);

    public TokenValidationParameters CloneParameters()
        => _defaultValidationParameters.Clone();
    
    public RefreshToken GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        var ip = _contextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString()
                 ?? "localhost";
        
        var useragent = _contextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString()
                        ?? "unknown";

        return new RefreshToken(
            Convert.ToBase64String(randomNumber),
            DateTime.UtcNow,
            DateTime.UtcNow.AddSeconds(AuthConfig.RefreshTokenLifetime),
            ip,
            useragent);
    }
}