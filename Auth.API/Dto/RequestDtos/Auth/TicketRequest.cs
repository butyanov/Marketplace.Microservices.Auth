using Auth.API.Dto.SupportTypes;
using Auth.API.Dto.SupportTypes.Auth;
using Auth.API.Services.SupportTypes;

namespace Auth.API.Dto.RequestDtos.Auth;

public record TicketRequest(string Credentials, TicketScopes Scope, TicketTypes Type = TicketTypes.Email)
    : TicketMetadata(Credentials, Scope, Type);