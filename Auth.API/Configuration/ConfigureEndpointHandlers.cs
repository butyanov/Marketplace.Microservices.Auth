using Auth.API.Dto.RequestDtos;
using Auth.API.Dto.RequestDtos.Auth;
using Auth.API.Dto.ResponseDtos;
using Auth.API.Dto.ResponseDtos.Auth;
using Auth.API.EndpointsHandlers.Auth;
using Auth.API.EndpointsHandlers.Interfaces;
using Auth.API.EndpointsHandlers.Me;
using Auth.API.EndpointsHandlers.Users;

namespace Auth.API.Configuration;

public static class ConfigureEndpointHandlers
{
    public static IServiceCollection
        AddCustomEndpointHandlersConfiguration(this IServiceCollection servicesCollection) =>
        servicesCollection
             // auth   
            .AddScoped<RefreshTokenEndpointHandler>()
            .AddScoped<LoginEndpointHandler>()
            .AddScoped<SignupEndpointHandler>()
            .AddScoped<IRequestResponseEndpointHandler<TicketRequest, TicketResponse>, TicketRequestEndpointHandler>()
            .AddScoped<LoginOrRegisterTicketEndpointHandler>()
            .AddScoped<AcquireTicketEndpointHandler>()
             // google auth
            .AddScoped<GoogleExchangeCodeOnTokenEndpointHandler>()
            .AddScoped<GoogleUserAuthenticationEndpointHandler>()
             // private user data control
            .AddScoped<MeGetEndpointHandler>()
            .AddScoped<MeUpdateEndpointHandler>()
            .AddScoped<MeDeleteEndpointHandler>()
             // users administered data moderation
            .AddScoped<UsersCreateEndpointHandler>()
            .AddScoped<UsersUpdateEndpointHandler>()
            .AddScoped<UsersDeleteEndpointHandler>()
            .AddScoped<UsersGetAllEndpointHandler>()
            .AddScoped<UsersGetEndpointHandler>()
            .AddScoped<UsersPromoteEndpointHandler>();
}