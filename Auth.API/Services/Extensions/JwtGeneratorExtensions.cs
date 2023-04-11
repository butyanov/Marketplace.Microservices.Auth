using System.Security.Claims;
using Auth.API.Dto.SupportTypes.Auth;
using Auth.API.Models;
using Auth.API.Services.Interfaces;
using Auth.API.Services.SupportTypes;

namespace Auth.API.Services.Extensions;

public static class JwtGeneratorExtensions
{
    public static string GenerateUserToken(this IJwtTokenGenerator generator, DomainUser user, PermissionsModel access, DateTime expiration) 
        => generator.GenerateFromClaims(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, access.Id.ToString()),
            new Claim("Permissions", ((uint) access.Permissions).ToString())
        }, expiration);

    public static TicketMetadata? ReadPhoneTicketRequest(this IJwtTokenGenerator generator, string token)
        => generator.ReadToken<TicketMetadata>(token, false);
}