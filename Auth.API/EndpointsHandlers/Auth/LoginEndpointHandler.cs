using Auth.API.Data.Interfaces;
using Auth.API.Dto.RequestDtos;
using Auth.API.Dto.ResponseDtos;
using Auth.API.EndpointsHandlers.Interfaces;
using Auth.API.Exceptions;

namespace Auth.API.EndpointsHandlers.Auth;

public class LoginEndpointHandler : IEndpointHandler<AuthorizationRequest, AuthorizationResponse>
{
    private readonly IDomainDbContext _dbContext;
    private readonly Services.AuthenticationService _authService;
    public LoginEndpointHandler(IDomainDbContext dbContext, Services.AuthenticationService authService)
    {
        _dbContext = dbContext;
        _authService = authService;
    }

    public async Task<AuthorizationResponse> Handle(AuthorizationRequest request)
    {
        var result = AuthorizationResponse.FromAuthenticationResult(
            await (
                request.LoginMode switch
                {
                    LoginMode.Phone => _authService.HandleTicketLogin(
                        request.PhoneTicket
                        ?? throw new ValidationFailedException(nameof(request.PhoneTicket), "TICKET_IS_NULL")),

                    LoginMode.Password => _authService.HandlePasswordLogin(
                        request.Email
                        ?? throw new ValidationFailedException(nameof(request.Email), "EMAIL_IS_NULL"),
                        request.Password
                        ?? throw new ValidationFailedException(nameof(request.Password), "PASSWORD_IS_NULL"))
                }));

        await _dbContext.SaveEntitiesAsync();
        return result;
    }
}