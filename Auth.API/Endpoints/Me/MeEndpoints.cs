using Auth.API.Dto.RequestDtos.User;
using Auth.API.Endpoints.Extensions.ValidationFilter;
using Auth.API.EndpointsHandlers.Me;

namespace Auth.API.Endpoints.Me;

public class MeEndpoints : IEndpoints
{
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/get", async (MeGetEndpointHandler handler) => 
            await handler.Handle()).RequireAuthorization("User");
        endpoints.MapPatch("/update", async (UserUpdateRequest request, MeUpdateEndpointHandler handler) => 
            await handler.Handle(request)).RequireAuthorization("User").AddValidation(c => c.AddFor<UserUpdateRequest>());
        endpoints.MapDelete("/delete", async (MeDeleteEndpointHandler handler) => 
            await handler.Handle()).RequireAuthorization("User");
    }
}