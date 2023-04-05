using Auth.API.Data.Configuration;
using Auth.API.Data.Configuration.Abstractions;
using Auth.API.Models;

namespace Auth.API.Configuration;

public static class ConfigureEntitiesConfiguration 
{
    public static IServiceCollection AddCustomEntitiesConfiguration(this IServiceCollection services) =>
        services
            .AddSingleton<DependencyInjectedEntityConfiguration, ApplicationUserConfiguration>()
            .AddSingleton<DependencyInjectedEntityConfiguration, DomainUserConfiguration>();
    
}