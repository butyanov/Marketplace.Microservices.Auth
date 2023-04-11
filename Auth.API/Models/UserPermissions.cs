using Auth.API.Services.SupportTypes;

namespace Auth.API.Models;

public record PermissionsModel(Guid Id, UserPermissions Permissions);