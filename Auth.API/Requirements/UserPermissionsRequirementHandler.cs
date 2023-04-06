using Auth.API.Exceptions;
using Auth.API.Services.SupportTypes;
using Microsoft.AspNetCore.Authorization;

namespace Auth.API.Requirements;

public class UserPermissionsRequirementHandler : AuthorizationHandler<UserPermissionsRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserPermissionsRequirementHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        UserPermissionsRequirement requirement)
    {
        if (!context.User.Identity!.IsAuthenticated)
            throw new UnauthorizedException("UNAUTHORIZED_ACCESS");
        if (context.User.HasClaim(c => c.Type == "Permissions"))
        {
            if(!IsPermitted(requirement.Permissions,
                   (UserPermissions)byte.Parse(context.User.Claims.First(c => c.Type == "Permissions").Value)))
                throw new ForbiddenException("FORBIDDEN");
            context.Succeed(requirement);
        }
        else
            throw new UnauthorizedException("UNAUTHORIZED_ACCESS");
    }

    private bool IsPermitted(UserPermissions requiredPermissions, UserPermissions userPermissions) =>
        (requiredPermissions & userPermissions) == requiredPermissions;
    
}