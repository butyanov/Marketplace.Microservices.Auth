using Auth.API.Services.SupportTypes;

namespace Auth.API.Dto.RequestDtos;

public record EmailLoginOrRegisterTicketRequest
    (string Credentials, TicketTypes Type = TicketTypes.Email) : LoginOrRegisterTicketRequest(Credentials, Type)
{
    public static EmailLoginOrRegisterTicketRequest FromTicketRequest(LoginOrRegisterTicketRequest request) => new(request.Credentials);
};