using System.Net.Mime;
using System.Security.Claims;
using Auth.API.Data;
using Auth.API.Data.Extensions;
using Auth.API.Dto.RequestDtos.User;
using Auth.API.EndpointsHandlers.Interfaces;
using Auth.API.Exceptions;
using Auth.API.InfrastructureExtensions;
using Auth.API.Models;
using Auth.API.Services.Interfaces;
using Auth.API.Services.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Auth.API.EndpointsHandlers.Me;

public class MeUpdateEndpointHandler : IRequestEndpointHandler<UserUpdateRequest>
{
    private readonly string _token;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly AuthDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public MeUpdateEndpointHandler(IHttpContextAccessor httpContextAccessor, IJwtTokenGenerator jwtTokenGenerator, AuthDbContext dbContext, IMapper mapper, UserManager<ApplicationUser> userManager)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _dbContext = dbContext;
        _userManager = userManager;
        _mapper = mapper;
        _token = httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
    }

    public async Task Handle(UserUpdateRequest request)
    {
        var userId =
            Guid.Parse(
                _jwtTokenGenerator.ReadToken(_token)
                    .FindFirst(x => x.Type == ClaimTypes.NameIdentifier)
                    ?.Value
                ?? throw new UnauthorizedException("INVALID_TOKEN"));
        
        var updatedDomainUser = _mapper.Map<UserUpdateRequest, DomainUser>(request);
        var currentDomainUser = await _dbContext.MarketUsers.FindAsync(userId);
        _dbContext.UpdateEntity(currentDomainUser ?? throw new NotFoundException<DomainUser>(), updatedDomainUser);

        var currentIdentityUser = await _userManager.FindByIdAsync(currentDomainUser.IdentityUserId.ToString()) 
                                  ?? throw new NotFoundException<DomainUser>();
        currentIdentityUser.UserName =
            $"{TextConverter.ConvertToLatin(currentDomainUser.Name)}{new Random().Next(10000, 99999)}";
        currentIdentityUser.NormalizedUserName = currentIdentityUser.UserName.ToUpperInvariant();
            
        await _userManager.UpdateAsync(currentIdentityUser);
        await _dbContext.SaveEntitiesAsync();
    }
}