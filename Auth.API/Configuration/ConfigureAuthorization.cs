using Auth.API.Requirements;
using Auth.API.Services.SupportTypes;
using Microsoft.AspNetCore.Authorization;

namespace Auth.API.Configuration;

public static class ConfigureAuthorization
{
    public static IServiceCollection AddCustomAuthorization(this IServiceCollection services) =>
        services
            .AddTransient<IAuthorizationHandler, UserPermissionsRequirementHandler>()
            .AddAuthorization(o =>
            {
                o.AddPolicy("User",
                    p => p.AddRequirements(new UserPermissionsRequirement(UserPermissionsPresets.User)));
                o.AddPolicy("Moderator",
                    p => p.AddRequirements(new UserPermissionsRequirement(UserPermissionsPresets.Moderator)));
                o.AddPolicy("Admin",
                    p => p.AddRequirements(new UserPermissionsRequirement(UserPermissionsPresets.Admin)));
                o.AddPolicy("Creator",
                    p => p.AddRequirements(new UserPermissionsRequirement(UserPermissionsPresets.Creator)));
                o.AddPolicy("WriteAndDelete",
                    p => p.AddRequirements(new UserPermissionsRequirement(UserPermissions.Write | UserPermissions.Delete)));
                o.AddPolicy("ReadAndEdit",
                    p => p.AddRequirements(new UserPermissionsRequirement(UserPermissions.Read | UserPermissions.Edit)));
            });

    
}