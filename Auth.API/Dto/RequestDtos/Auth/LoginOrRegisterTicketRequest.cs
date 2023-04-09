using Auth.API.Services.SupportTypes;

namespace Auth.API.Dto.RequestDtos.Auth;

public record LoginOrRegisterTicketRequest(string Credentials, TicketTypes Type);
