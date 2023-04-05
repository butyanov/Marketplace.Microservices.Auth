using Auth.API.Dto.RequestDtos;
using Auth.API.Dto.ResponseDtos;
using Auth.API.EndpointsHandlers.Auth;
using Auth.API.EndpointsHandlers.Interfaces;

namespace Auth.API.Configuration;

public static class ConfigureEndpointHandlers
{
    public static IServiceCollection
        AddCustomEndpointHandlersConfiguration(this IServiceCollection servicesCollection) =>
        servicesCollection
            .AddScoped<RefreshTokenEndpointHandler>()
            .AddScoped<LoginEndpointHandler>()
            .AddScoped<SignupEndpointHandler>()
            .AddScoped<IEndpointHandler<TicketRequest, TicketResponse>, TicketRequestEndpointHandler>()
            .AddScoped<LoginOrRegisterTicketEndpointHandler>()
            .AddScoped<AcquireTicketEndpointHandler>();
}