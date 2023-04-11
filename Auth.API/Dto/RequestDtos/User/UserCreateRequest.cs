namespace Auth.API.Dto.RequestDtos.User;

public record UserCreateRequest(string Name, string? Country, string? PostalIndex, string Email, string? PhoneNumber, string Password);