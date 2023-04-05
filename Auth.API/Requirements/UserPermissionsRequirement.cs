using Auth.API.Services.SupportTypes;
using Microsoft.AspNetCore.Authorization;

namespace Auth.API.Requirements;

public class UserPermissionsRequirement : IAuthorizationRequirement
{
    public UserPermissions Permissions { get; set; }
    public UserPermissionsRequirement(UserPermissions permissions) => Permissions = permissions;

}