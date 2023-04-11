using Auth.API.Data;
using Auth.API.Dto.RequestDtos.User;
using Auth.API.EndpointsHandlers.Interfaces;
using Auth.API.Exceptions;
using Auth.API.InfrastructureExtensions;
using Auth.API.Models;
using Auth.API.Services.Interfaces;
using Auth.API.Services.SupportTypes;

namespace Auth.API.EndpointsHandlers.Users;

public class UsersPromoteEndpointHandler : IRequestEndpointHandler<UserPromoteRequest>
{
    private readonly string _token;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly AuthDbContext _dbContext;
   
    
    public UsersPromoteEndpointHandler(IHttpContextAccessor httpContextAccessor, IJwtTokenGenerator jwtTokenGenerator, AuthDbContext dbContext)
    {
        _token = httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
        _jwtTokenGenerator = jwtTokenGenerator;
        _dbContext = dbContext;
    }

    public async Task Handle(UserPromoteRequest request)
    {
        var promoterAccess =
            (UserPermissions) uint.Parse(_jwtTokenGenerator.ReadToken(_token)
                                             .FindFirst(x => x.Type == "Permissions")
                                             ?.Value
                                         ?? throw new UnauthorizedException("INVALID_TOKEN"));
        
        if (!IsPermitted(promoterAccess, request.Permissions))
            throw new UnauthorizedAccessException();
        
        var userId = (await _dbContext.MarketUsers.FirstOrNotFoundAsync(u => u.Id == request.UserId)).Id;
        await _dbContext.Permissions.AddAsync(new PermissionsModel(userId, request.Permissions));
        
        await _dbContext.SaveEntitiesAsync();
    }
    
    private bool IsPermitted(UserPermissions promoterAccess, UserPermissions promotedPermissions) =>
        (promotedPermissions & promoterAccess) == promotedPermissions;
}