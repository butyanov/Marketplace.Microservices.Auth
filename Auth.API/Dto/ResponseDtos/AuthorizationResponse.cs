using Auth.API.Models.SupportTypes;

namespace Auth.API.Dto.ResponseDtos;

public record AuthorizationResponse(string Token, string RefreshToken)
{
    public static AuthorizationResponse FromAuthenticationResult(AuthenticationResult authenticationResult)
    {
        return new AuthorizationResponse(authenticationResult.Token, authenticationResult.RefreshToken.Token);
    }
}