namespace Auth.API.Dto.ResponseDtos.Auth;

public record TicketResponse(DateTime NextTry, bool CodeSent);