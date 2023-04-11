using Auth.API.Endpoints;
using Auth.API.Endpoints.Auth;
using Auth.API.Endpoints.Me;
using Auth.API.Endpoints.Users;

namespace Auth.API.Configuration;

public static class ConfigureApi
{
    public static IServiceCollection AddCustomApiConfiguration(this IServiceCollection servicesCollection) => 
        servicesCollection
            .AddEndpointsApiExplorer()
            .AddTransient<IEndpointsRoot, AuthEndpointsRoot>()
            .AddTransient<IEndpointsRoot, MeEndpointsRoot>()
            .AddTransient<IEndpointsRoot, UsersEndpointsRoot>();
}