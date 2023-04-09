using Auth.API.Dto.SupportTypes.Auth;

namespace Auth.API.Dto.ResponseDtos.Auth;

public record AuthorizationResponse(string Token, string RefreshToken)
{
    public static AuthorizationResponse FromAuthenticationResult(AuthenticationResult authenticationResult)
    {
        return new AuthorizationResponse(authenticationResult.Token, authenticationResult.RefreshToken.Token);
    }
}