using Auth.API.Dto.SupportTypes;
using Auth.API.Services.SupportTypes;

namespace Auth.API.Dto.RequestDtos;

public record TicketRequest(string Credentials, TicketScopes Scope, TicketTypes Type = TicketTypes.Email)
    : TicketMetadata(Credentials, Scope, Type);