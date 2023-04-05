namespace Auth.API.Dto.ResponseDtos;

public record TicketResponse(DateTime NextTry, bool CodeSent);