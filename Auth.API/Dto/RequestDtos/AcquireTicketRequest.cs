using Auth.API.Dto.SupportTypes;
using Auth.API.Services.SupportTypes;

namespace Auth.API.Dto.RequestDtos;

public record AcquireTicketRequest(string Credentials, TicketScopes Scope, TicketTypes Type, string Code) : TicketMetadata(Credentials, Scope, Type);