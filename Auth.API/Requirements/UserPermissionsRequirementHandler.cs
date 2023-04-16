using Auth.API.Exceptions;
using Auth.API.Services.SupportTypes;
using Microsoft.AspNetCore.Authorization;

namespace Auth.API.Requirements;

public class UserPermissionsRequirementHandler : AuthorizationHandler<UserPermissionsRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        UserPermissionsRequirement requirement)
    {
        if (!context.User.Identity!.IsAuthenticated)
            throw new UnauthorizedException("UNAUTHENTICATED_USER");
        
        if(!IsPermitted(requirement.Permissions, (UserPermissions) uint.Parse(context.User.Claims.First(c => c.Type == "Permissions").Value)))
            throw new UnauthorizedAccessException();
        
        context.Succeed(requirement);
    }

    private bool IsPermitted(UserPermissions requiredPermissions, UserPermissions userPermissions) =>
        (requiredPermissions & userPermissions) == requiredPermissions;
}