using Auth.API.Data;
using Auth.API.Dto.RequestDtos.User;
using Auth.API.EndpointsHandlers.Interfaces;
using Auth.API.Exceptions;
using Auth.API.Models;
using Auth.API.Services.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Auth.API.EndpointsHandlers.Users;

public class UsersCreateEndpointHandler : IRequestEndpointHandler<UserCreateRequest>
{
    private readonly AuthDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public UsersCreateEndpointHandler(AuthDbContext dbContext, IMapper mapper, UserManager<ApplicationUser> userManager)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _userManager = userManager;
    }
    
    public async Task Handle(UserCreateRequest request)
    {
        var domainUser = _mapper.Map<UserCreateRequest, DomainUser>(request);
        var identityUser = _mapper.Map<DomainUser, ApplicationUser>(domainUser);
        domainUser.IdentityUserId = identityUser.Id;
        identityUser.UserName = $"{TextConverter.ConvertToLatin(domainUser.Name)}{new Random().Next(1000, 999999)}";
        
        await _dbContext.MarketUsers.AddAsync(domainUser);
        var result = await _userManager.CreateAsync(identityUser, request.Password);
        if (!result.Succeeded)
            throw new UnauthorizedException(string.Join("\n", result.Errors.Select(x => x.Description)));

        await _dbContext.SaveEntitiesAsync();
    }
    
}