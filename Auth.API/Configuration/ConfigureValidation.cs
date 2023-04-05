using FluentValidation;
using FluentValidation.AspNetCore;

namespace Auth.API.Configuration;

public static class ConfigureValidation
{
    public static IServiceCollection AddCustomValidation(this IServiceCollection services) =>
        services
            .AddFluentValidationAutoValidation().AddValidatorsFromAssembly(typeof(Program).Assembly);
  

}