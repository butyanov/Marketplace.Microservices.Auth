using Auth.API.Services.SupportTypes;

namespace Auth.API.Dto.RequestDtos;

public record PersistedTicketRequest(
    string Credentials,
    string Code,
    DateTime NextTry,
    TicketScopes Scope,
    TicketTypes Type);