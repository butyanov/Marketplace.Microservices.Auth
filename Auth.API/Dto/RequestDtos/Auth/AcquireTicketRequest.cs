using Auth.API.Dto.SupportTypes;
using Auth.API.Dto.SupportTypes.Auth;
using Auth.API.Services.SupportTypes;

namespace Auth.API.Dto.RequestDtos.Auth;

public record AcquireTicketRequest(string Credentials, TicketScopes Scope, TicketTypes Type, string Code) : TicketMetadata(Credentials, Scope, Type);