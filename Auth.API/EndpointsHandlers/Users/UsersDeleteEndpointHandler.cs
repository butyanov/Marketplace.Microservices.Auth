using Auth.API.Data;
using Auth.API.EndpointsHandlers.Interfaces;
using Auth.API.Exceptions;
using Auth.API.InfrastructureExtensions;
using Auth.API.Models;
using Microsoft.AspNetCore.Identity;

namespace Auth.API.EndpointsHandlers.Users;

public class UsersDeleteEndpointHandler : IRequestEndpointHandler<Guid>
{
    private readonly AuthDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public UsersDeleteEndpointHandler(AuthDbContext dbContext, UserManager<ApplicationUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }
    public async Task Handle(Guid request)
    {
        var domainUser = await _dbContext.MarketUsers.FirstOrNotFoundAsync(u => u.Id == request);
        _dbContext.MarketUsers.Remove(domainUser);
        
        var identityUser = await _userManager.FindByIdAsync(domainUser.IdentityUserId.ToString()) ?? throw new NotFoundException<ApplicationUser>();
        await _userManager.DeleteAsync(identityUser);
        
        await _dbContext.SaveEntitiesAsync();
    }
}