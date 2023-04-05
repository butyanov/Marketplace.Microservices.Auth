using Auth.API.Services.SupportTypes;

namespace Auth.API.Dto.SupportTypes;

public record TicketMetadata(string Credentials, TicketScopes Scope, TicketTypes Type = TicketTypes.Email)
{
    public string GetRedisKey() => $"{Credentials}:{Scope}:{Type}";
}