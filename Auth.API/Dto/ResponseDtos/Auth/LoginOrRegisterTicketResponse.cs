namespace Auth.API.Dto.ResponseDtos.Auth;

public record LoginOrRegisterTicketResponse(DateTime NextTry, bool CodeSent, bool IsLogin) : TicketResponse(NextTry, CodeSent);