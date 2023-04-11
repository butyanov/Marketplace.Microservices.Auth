using Auth.API.Data;
using Auth.API.EndpointsHandlers.Interfaces;
using Auth.API.InfrastructureExtensions;
using Auth.API.Models;

namespace Auth.API.EndpointsHandlers.Users;

public class UsersGetEndpointHandler : IRequestResponseEndpointHandler<Guid, DomainUser>
{
    private readonly AuthDbContext _dbContext;

    public UsersGetEndpointHandler(AuthDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DomainUser> Handle(Guid request) =>
        await _dbContext.MarketUsers.FirstOrNotFoundAsync(u => u.Id == request);
    
}