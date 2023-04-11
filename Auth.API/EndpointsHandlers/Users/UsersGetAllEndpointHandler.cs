using Auth.API.Data;
using Auth.API.EndpointsHandlers.Interfaces;
using Auth.API.Models;

namespace Auth.API.EndpointsHandlers.Users;

public class UsersGetAllEndpointHandler : IResponseEndpointHandler<IQueryable<DomainUser>>
{
    private readonly AuthDbContext _dbContext;

    public UsersGetAllEndpointHandler(AuthDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IQueryable<DomainUser>> Handle() =>
        _dbContext.MarketUsers.AsQueryable();
}