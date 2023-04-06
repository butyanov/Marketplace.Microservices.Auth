namespace Auth.API.Endpoints.Auth;

public class AuthEndpointsRoot : IEndpointsRoot
{
    public void MapEndpoints(WebApplication app)
    {
        var group = (IEndpointRouteBuilder)app.MapGroup($"/auth/v{app.Configuration["ApiData:Version"]}")
            .WithTags("Авторизация");
        group
            .AddEndpoints<AuthEndpoints>();
    }
}