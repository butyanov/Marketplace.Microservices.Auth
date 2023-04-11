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
            .AddScoped<RefreshTokenEndpointHandler>()
            .AddScoped<LoginEndpointHandler>()
            .AddScoped<SignupEndpointHandler>()
            .AddScoped<IRequestResponseEndpointHandler<TicketRequest, TicketResponse>, TicketRequestEndpointHandler>()
            .AddScoped<LoginOrRegisterTicketEndpointHandler>()
            .AddScoped<AcquireTicketEndpointHandler>()
            .AddScoped<MeGetEndpointHandler>()
            .AddScoped<MeUpdateEndpointHandler>()
            .AddScoped<MeDeleteEndpointHandler>()
            .AddScoped<UsersCreateEndpointHandler>()
            .AddScoped<UsersUpdateEndpointHandler>()
            .AddScoped<UsersDeleteEndpointHandler>()
            .AddScoped<UsersGetAllEndpointHandler>()
            .AddScoped<UsersGetEndpointHandler>()
            .AddScoped<UsersPromoteEndpointHandler>();
}