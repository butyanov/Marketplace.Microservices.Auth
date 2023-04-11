namespace Auth.API.Dto.RequestDtos.Auth;

public record AuthorizationRequest(
    string? Ticket,
    string? Login,
    string? Password,
    LoginMode LoginMode,
    LoginType LoginType
);

public record EmailAuthorizationRequest(
    string? Ticket,
    string? Login,
    string? Password,
    LoginMode LoginMode,
    LoginType LoginType = LoginType.Email
    ) 
    : AuthorizationRequest(Ticket, Login, Password, LoginMode, LoginType)
{
    public static EmailAuthorizationRequest FromAuthorizationRequest(AuthorizationRequest authorizationRequest) =>
        new(authorizationRequest.Ticket, authorizationRequest.Login, authorizationRequest.Password,
            authorizationRequest.LoginMode);

}
public record PhoneAuthorizationRequest(
        string? Ticket,
        string? Login,
        string? Password,
        LoginMode LoginMode,
        LoginType LoginType = LoginType.Phone
    ) 
    : AuthorizationRequest(Ticket, Login, Password, LoginMode, LoginType)
{
    public static PhoneAuthorizationRequest FromAuthorizationRequest(AuthorizationRequest authorizationRequest) =>
        new(authorizationRequest.Ticket, authorizationRequest.Login, authorizationRequest.Password,
            authorizationRequest.LoginMode);

}

public enum LoginMode
{
    Phone,
    Password
}
public enum LoginType
{
    Email,
    Phone
}