using Auth.API.Data;
using Auth.API.Data.Interfaces;
using Auth.API.EndpointsHandlers.Auth;
using Auth.API.Services;
using Auth.API.Services.Interfaces;

namespace Auth.API.Configuration;

public static class ConfigureServices
{
    public static IServiceCollection AddCustomServicesConfiguration(this IServiceCollection servicesCollection) =>
        servicesCollection
            .AddAutoMapper(typeof(Program).Assembly)
            .AddScoped(typeof(IRedisStore<>), typeof(RedisStore<>))
            .AddScoped<IRedisKeyStore, RedisKeyStore>()
            .AddScoped<IVerificationCodeGeneratorService, DigitVerificationCodeGeneratorService>()
            .AddScoped<ISenderService, EmailSenderService>()
            .AddScoped<IJwtTokenGenerator, JwtTokenGenerator>()
            .AddScoped<AuthenticationService>()
            .AddScoped<LoginEndpointHandler>()
            .AddScoped<SignupEndpointHandler>();
}