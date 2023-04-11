namespace Auth.API.Dto.RequestDtos.User;

public record UserAdvancedUpdateRequest(Guid Id, string? Name, string? Email, string? PhoneNumber, string? Country,
    string? PostalIndex) : UserUpdateRequest(Name, Country, PostalIndex);