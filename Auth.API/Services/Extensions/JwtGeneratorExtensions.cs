using System.Security.Claims;
using Auth.API.Dto.SupportTypes.Auth;
using Auth.API.Models;
using Auth.API.Services.Interfaces;
using Auth.API.Services.SupportTypes;

namespace Auth.API.Services.Extensions;

public static class JwtGeneratorExtensions
{
    public static string GenerateUserToken(this IJwtTokenGenerator generator, DomainUser user, DateTime expiration) 
        => generator.GenerateFromClaims(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("Permissions", ((byte) UserPermissionsPresets.User).ToString())
        }, expiration);

    public static TicketMetadata? ReadPhoneTicketRequest(this IJwtTokenGenerator generator, string token)
        => generator.ReadToken<TicketMetadata>(token, false);
}