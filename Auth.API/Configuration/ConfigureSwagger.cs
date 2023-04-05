using Microsoft.OpenApi.Models;

namespace Auth.API.Configuration;

public static class ConfigureSwagger
{
    public static IServiceCollection AddCustomSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { 
                Title = "My API", 
                Version = "v1" 
            });
            c.AddSecurityDefinition(
                "bearerAuth",
                new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                { 
                    new OpenApiSecurityScheme 
                    { 
                        Reference = new OpenApiReference 
                        { 
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer" 
                        } 
                    },
                    new string[] { } 
                } 
            });
        });
        
        return services;
    }
}