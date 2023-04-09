namespace Auth.API.Dto.ResponseDtos.User;

public record UserAdvancedResponse(Guid Id, string Name, string? PhoneNumber, string? Email, string? Country,
    string? PostalIndex) : UserResponse(Name, PhoneNumber, Email, Country, PostalIndex);
