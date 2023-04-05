namespace Auth.API.Endpoints.Auth;

public class AuthEndpointsRoot : IEndpointsRoot
{
    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/auth")
            .WithTags("Авторизация");
        group
            .AddEndpoints<AuthEndpoints>();
    }
}