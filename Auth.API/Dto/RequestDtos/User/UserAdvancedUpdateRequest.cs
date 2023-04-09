namespace Auth.API.Dto.RequestDtos.User;

public record UserAdvancedUpdateRequest(Guid Id, string? Name, string? Country,
    string? PostalIndex) : UserUpdateRequest(Name, Country, PostalIndex);