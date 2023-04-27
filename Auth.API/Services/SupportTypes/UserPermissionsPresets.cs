namespace Auth.API.Services.SupportTypes;

public static class UserPermissionsPresets
{
    public const UserPermissions Anonymous = UserPermissions.Read;
    public const UserPermissions User = UserPermissions.Read | UserPermissions.Edit;
    public const UserPermissions Moderator = UserPermissions.Read | UserPermissions.Edit | UserPermissions.Write;
    public const UserPermissions Admin = UserPermissions.Read | UserPermissions.Edit | UserPermissions.Write | UserPermissions.Delete;
    public const UserPermissions Creator = (UserPermissions) byte.MaxValue;
}