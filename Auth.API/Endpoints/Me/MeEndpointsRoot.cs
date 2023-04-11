namespace Auth.API.Endpoints.Me;

public class MeEndpointsRoot : IEndpointsRoot
{
    public void MapEndpoints(WebApplication app)
    {
        var group = (IEndpointRouteBuilder)app.MapGroup($"/me/v{app.Configuration["ApiData:Version"]}")
            .WithTags("Личные данные");
        group
            .AddEndpoints<MeEndpoints>();
    }
}