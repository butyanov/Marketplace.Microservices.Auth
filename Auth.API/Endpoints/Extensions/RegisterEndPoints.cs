namespace Auth.API.Endpoints;

public static class RegisterEndPoints
{
     public static IEndpointRouteBuilder AddEndpoints<T>(this IEndpointRouteBuilder routeBuilder)
            where T : IEndpoints
        {
            var instance = ActivatorUtilities.CreateInstance<T>(routeBuilder.ServiceProvider);
            instance.Map(routeBuilder);
            return routeBuilder;
        }
}