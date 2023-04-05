using System.Text.Json;
using Microsoft.Extensions.Options;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Core.Implementations;
using StackExchange.Redis.Extensions.System.Text.Json;

namespace Auth.API.Configuration;

public static class ConfigureRedis
{
    public static IServiceCollection AddRedisConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IRedisClientFactory, RedisClientFactory>();

        services.AddSingleton<ISerializer, SystemTextJsonSerializer>(
            (provider) => new SystemTextJsonSerializer(
                provider.GetRequiredService<IOptions<JsonSerializerOptions>>().Value));

        services.AddSingleton<IRedisClient>(
            provider
                => provider.GetRequiredService<IRedisClientFactory>().GetDefaultRedisClient());

        services.AddSingleton<IRedisDatabase>(
            provider
                => provider.GetRequiredService<IRedisClientFactory>().GetDefaultRedisClient().GetDefaultDatabase());

        services.AddSingleton<IEnumerable<RedisConfiguration>>(
            (_) =>
            {
                return new RedisConfiguration[]
                {
                    new()
                    {
                        ConnectionString = configuration.GetConnectionString("RedisConnectionString"),
                    }
                };
            });

        return services;
    }
}