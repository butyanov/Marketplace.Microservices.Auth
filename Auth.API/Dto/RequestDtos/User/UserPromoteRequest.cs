using Auth.API.Services.SupportTypes;

namespace Auth.API.Dto.RequestDtos.User;

public record UserPromoteRequest(Guid UserId, UserPermissions Permissions);