namespace Auth.API.Dto.ResponseDtos;

public record LoginOrRegisterTicketResponse(DateTime NextTry, bool CodeSent, bool IsLogin) : TicketResponse(NextTry, CodeSent);