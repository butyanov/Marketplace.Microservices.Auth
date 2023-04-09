namespace Auth.API.Dto.ResponseDtos.User;

public record UserResponse(string Name, string? PhoneNumber, string? Email, string? Country, string? PostalIndex);
