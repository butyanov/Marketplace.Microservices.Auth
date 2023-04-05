using Auth.API.Endpoints;
using Auth.API.Endpoints.Auth;

namespace Auth.API.Configuration;

public static class ConfigureApi
{
    public static IServiceCollection AddCustomApiConfiguration(this IServiceCollection servicesCollection) => 
        servicesCollection
            .AddEndpointsApiExplorer()
            .AddTransient<IEndpointsRoot, AuthEndpointsRoot>();
}