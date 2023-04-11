using Auth.API.Data.Interfaces;
using Auth.API.Dto.RequestDtos;
using Auth.API.Dto.RequestDtos.Auth;
using Auth.API.Dto.ResponseDtos;
using Auth.API.Dto.ResponseDtos.Auth;
using Auth.API.EndpointsHandlers.Interfaces;
using Auth.API.Exceptions;
using Azure.Core;

namespace Auth.API.EndpointsHandlers.Auth;

public class LoginEndpointHandler : IRequestResponseEndpointHandler<AuthorizationRequest, AuthorizationResponse>
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
                    LoginMode.Phone => _authService.ProcessTicketLogin(
                        request.Ticket
                        ?? throw new ValidationFailedException(nameof(request.Ticket), "TICKET_IS_NULL")),

                    LoginMode.Password => _authService.ProcessPasswordLogin(
                        request.LoginType,
                        request.Login 
                        ?? throw new ValidationFailedException(nameof(request.Login), "LOGIN_IS_NULL"),
                        request.Password
                        ?? throw new ValidationFailedException(nameof(request.Password), "PASSWORD_IS_NULL"))
                }));

        await _dbContext.SaveEntitiesAsync();
        return result;
    }
}