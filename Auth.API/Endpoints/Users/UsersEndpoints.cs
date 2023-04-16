using Auth.API.Dto.RequestDtos.User;
using Auth.API.Endpoints.Extensions.ValidationFilter;
using Auth.API.EndpointsHandlers.Users;

namespace Auth.API.Endpoints.Users;

public class UsersEndpoints : IEndpoints
{
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/all", async (UsersGetAllEndpointHandler handler) => 
            await handler.Handle()).RequireAuthorization("Moderator");
        endpoints.MapGet("/get", async (Guid id, UsersGetEndpointHandler handler) => 
            await handler.Handle(id)).RequireAuthorization("Moderator");
        endpoints.MapPost("/create", async (UserCreateRequest request, UsersCreateEndpointHandler handler) => 
            await handler.Handle(request)).RequireAuthorization("Admin").AddValidation(c => c.AddFor<UserCreateRequest>());
        endpoints.MapPatch("/update", async (UserAdvancedUpdateRequest request, UsersUpdateEndpointHandler handler) => 
            await handler.Handle(request)).RequireAuthorization("Admin").AddValidation(c => c.AddFor<UserAdvancedUpdateRequest>());
        endpoints.MapDelete("/delete", async (Guid id, UsersDeleteEndpointHandler handler) => 
            await handler.Handle(id)).RequireAuthorization("Admin");
        endpoints.MapPost("/promote", async (UserPromoteRequest request, UsersPromoteEndpointHandler handler) =>
            await handler.Handle(request)).RequireAuthorization("Moderator");
    }
}