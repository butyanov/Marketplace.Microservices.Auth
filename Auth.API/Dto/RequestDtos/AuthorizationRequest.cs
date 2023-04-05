namespace Auth.API.Dto.RequestDtos;

public record AuthorizationRequest(
    LoginMode LoginMode,
    string? PhoneTicket,
    string Email,
    string Password
);
public enum LoginMode
{
    Phone,
    Password
}