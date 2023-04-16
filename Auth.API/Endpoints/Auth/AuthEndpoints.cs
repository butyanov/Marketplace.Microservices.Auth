using Auth.API.Dto.RequestDtos.Auth;
using Auth.API.Endpoints.Extensions.ValidationFilter;
using Auth.API.EndpointsHandlers.Auth;

namespace Auth.API.Endpoints.Auth;

public class AuthEndpoints : IEndpoints
{
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/token-refresh",
            async (RefreshTokenRequest request, RefreshTokenEndpointHandler handler) =>
                await handler.Handle(request)).AddValidation(c => c.AddFor<RefreshTokenRequest>());
        
        endpoints.MapPost("/email-login", async (AuthorizationRequest request, LoginEndpointHandler handler) => 
            await handler.Handle(EmailAuthorizationRequest.FromAuthorizationRequest(request)))
            .AddValidation(c => c.AddFor<EmailAuthorizationRequest>());
        
        endpoints.MapPost("/phone-login", async (AuthorizationRequest request, LoginEndpointHandler handler) => 
            await handler.Handle(PhoneAuthorizationRequest.FromAuthorizationRequest(request)))
            .AddValidation(c => c.AddFor<PhoneAuthorizationRequest>());
        
        endpoints.MapPost("/signup", async (SignupRequest request, SignupEndpointHandler handler) =>
            await handler.Handle(request)).AddValidation(c => c.AddFor<SignupRequest>());
        
        endpoints.MapPost("/init-phone-auth",
            async (LoginOrRegisterTicketRequest request, LoginOrRegisterTicketEndpointHandler handler) =>
                await handler.Handle(PhoneLoginOrRegisterTicketRequest.FromTicketRequest(request)))
            .AddValidation(c => c.AddFor<PhoneLoginOrRegisterTicketRequest>());
        
        endpoints.MapPost("/init-email-auth",
            async (LoginOrRegisterTicketRequest request, LoginOrRegisterTicketEndpointHandler handler) =>
                await handler.Handle(EmailLoginOrRegisterTicketRequest.FromTicketRequest(request)))
            .AddValidation(c => c.AddFor<EmailLoginOrRegisterTicketRequest>());
        
        endpoints.MapPost("/verify-auth-ticket",
            async (AcquireTicketRequest request, AcquireTicketEndpointHandler handler) =>
                await handler.Handle(request));

        endpoints.MapGet("/google-code-token", async (string code, GoogleExchangeCodeOnTokenEndpointHandler handler) =>
            await handler.Handle(code));

        endpoints.MapGet("/google-token-auth", async (string token, GoogleUserAuthenticationEndpointHandler handler) =>
            await handler.Handle(token));
    }
}