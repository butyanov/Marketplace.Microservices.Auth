namespace Auth.API.Configuration;

public static class ConfigureMiddleware
{
    public static void UseCustomMiddleware<T>(this WebApplication app)
    {
        app.UseMiddleware<T>();
    }
}