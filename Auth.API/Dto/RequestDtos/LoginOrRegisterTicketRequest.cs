using Auth.API.Services.SupportTypes;

namespace Auth.API.Dto.RequestDtos;

public record LoginOrRegisterTicketRequest(string Credentials, TicketTypes Type);
