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

namespace Auth.API.EndpointsHandlers.Users;

public class UsersUpdateEndpointHandler : IRequestEndpointHandler<UserAdvancedUpdateRequest>
{
    private readonly AuthDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public UsersUpdateEndpointHandler(AuthDbContext dbContext, IMapper mapper, UserManager<ApplicationUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task Handle(UserAdvancedUpdateRequest request)
    {
        var updatedDomainUser = _mapper.Map<UserAdvancedUpdateRequest, DomainUser>(request);
        var currentDomainUser = await _dbContext.MarketUsers.FirstOrNotFoundAsync(u => u.Id == request.Id);
        _dbContext.UpdateEntity(currentDomainUser, updatedDomainUser);

        var currentIdentityUser = await _userManager.FindByIdAsync(currentDomainUser.IdentityUserId.ToString()) 
                                  ?? throw new NotFoundException<DomainUser>();
        currentIdentityUser.UserName =
            $"{TextConverter.ConvertToLatin(currentDomainUser.Name)}{new Random().Next(10000, 99999)}";
        currentIdentityUser.NormalizedUserName = currentIdentityUser.UserName.ToUpperInvariant();
        if (request.Email != null)
        {
            currentIdentityUser.Email = request.Email;
            currentIdentityUser.NormalizedEmail = request.Email.ToUpperInvariant();
        }
        if (request.PhoneNumber != null)
            currentIdentityUser.PhoneNumber = request.PhoneNumber;
        
            
        await _userManager.UpdateAsync(currentIdentityUser);
        await _dbContext.SaveEntitiesAsync();
    }
}