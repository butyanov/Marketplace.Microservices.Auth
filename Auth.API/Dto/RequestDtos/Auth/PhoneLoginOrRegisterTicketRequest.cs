using Auth.API.Services.SupportTypes;

namespace Auth.API.Dto.RequestDtos.Auth;

public record PhoneLoginOrRegisterTicketRequest
    (string Credentials, TicketTypes Type = TicketTypes.Phone) : LoginOrRegisterTicketRequest(Credentials, Type)
{
    public static PhoneLoginOrRegisterTicketRequest FromTicketRequest(LoginOrRegisterTicketRequest request) => new(request.Credentials);
};
