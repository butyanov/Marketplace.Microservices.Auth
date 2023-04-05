namespace Auth.API.Models.SupportTypes;

public record AuthenticationResult(string Token, RefreshToken RefreshToken);
