using Auth.API.Services.SupportTypes;

namespace Auth.API.Dto.RequestDtos.Auth;

public record PersistedTicketRequest(
    string Credentials,
    string Code,
    DateTime NextTry,
    TicketScopes Scope,
    TicketTypes Type);