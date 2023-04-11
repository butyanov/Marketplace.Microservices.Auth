namespace Auth.API.Endpoints.Users;

public class UsersEndpointsRoot : IEndpointsRoot
{
    public void MapEndpoints(WebApplication app)
    {
        var group = (IEndpointRouteBuilder)app.MapGroup($"/users/v{app.Configuration["ApiData:Version"]}")
            .WithTags("Пользователи");
        group
            .AddEndpoints<UsersEndpoints>();
    }
}