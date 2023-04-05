namespace Auth.API.Dto.RequestDtos;

public record RefreshTokenRequest(string Token, string RefreshToken);