using System.Security.Claims;
using Auth.API.Data;
using Auth.API.EndpointsHandlers.Interfaces;
using Auth.API.Exceptions;
using Auth.API.InfrastructureExtensions;
using Auth.API.Models;
using Auth.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Auth.API.EndpointsHandlers.Me;

public class MeDeleteEndpointHandler : IEndpointHandler
{
    private readonly string _token;
    private readonly AuthDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private IJwtTokenGenerator _jwtTokenGenerator;

    public MeDeleteEndpointHandler(IHttpContextAccessor httpContextAccessor, AuthDbContext dbContext, UserManager<ApplicationUser> userManager, IJwtTokenGenerator jwtTokenGenerator)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _token = httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
    }
    public async Task Handle()
    {
        var userId =
            Guid.Parse(
                _jwtTokenGenerator.ReadToken(_token)
                    .FindFirst(x => x.Type == ClaimTypes.NameIdentifier)
                    ?.Value
                ?? throw new UnauthorizedException("INVALID_TOKEN"));
        
        var domainUser = await _dbContext.MarketUsers.FirstOrNotFoundAsync(u => u.Id == userId);
        _dbContext.MarketUsers.Remove(domainUser);
        
        var identityUser = await _userManager.FindByIdAsync(domainUser.IdentityUserId.ToString()) ?? throw new NotFoundException<ApplicationUser>();
        await _userManager.DeleteAsync(identityUser);
        
        await _dbContext.SaveEntitiesAsync();
    }
}