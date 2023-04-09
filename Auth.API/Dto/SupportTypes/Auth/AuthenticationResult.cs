using Auth.API.Models;

namespace Auth.API.Dto.SupportTypes.Auth;

public record AuthenticationResult(string Token, RefreshToken RefreshToken);
