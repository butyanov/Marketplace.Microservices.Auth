using Auth.API.Dto.RequestDtos;
using Auth.API.Endpoints.Extensions.ValidationFilter;
using Auth.API.EndpointsHandlers.Auth;

namespace Auth.API.Endpoints.Auth;

public class AuthEndpoints : IEndpoints
{
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/token-refresh",
            async (RefreshTokenRequest request, RefreshTokenEndpointHandler handler) =>
                Results.Ok(await handler.Handle(request)));
        
        endpoints.MapPost("/login", async (AuthorizationRequest request, LoginEndpointHandler handler) => 
            Results.Ok(await handler.Handle(request))).AddValidation(c => c.AddFor<AuthorizationRequest>());
        
        endpoints.MapPost("/signup", async (SignupRequest request, SignupEndpointHandler handler) =>
            Results.Ok(await handler.Handle(request))).AddValidation(c => c.AddFor<SignupRequest>());
        
        endpoints.MapPost("/init-phone-auth",
            async (PhoneLoginOrRegisterTicketRequest request, LoginOrRegisterTicketEndpointHandler handler) =>
                Results.Ok(await handler.Handle(PhoneLoginOrRegisterTicketRequest.FromTicketRequest(request))))
            .AddValidation(c => c.AddFor<PhoneLoginOrRegisterTicketRequest>());
        
        endpoints.MapPost("/init-email-auth",
            async (EmailLoginOrRegisterTicketRequest request, LoginOrRegisterTicketEndpointHandler handler) =>
                Results.Ok(await handler.Handle(EmailLoginOrRegisterTicketRequest.FromTicketRequest(request))))
            .AddValidation(c => c.AddFor<EmailLoginOrRegisterTicketRequest>());
        
        endpoints.MapPost("/verify-auth-ticket",
            async (AcquireTicketRequest request, AcquireTicketEndpointHandler handler) =>
                Results.Ok(await handler.Handle(request)));
    }
}