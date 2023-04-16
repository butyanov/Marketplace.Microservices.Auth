using Auth.API.Data;
using Auth.API.Data.Interfaces;
using Auth.API.Dto.RequestDtos.Auth;
using Auth.API.Dto.SupportTypes.Auth;
using Auth.API.Exceptions;
using Auth.API.InfrastructureExtensions;
using Auth.API.Models;
using Auth.API.Services.Extensions;
using Auth.API.Services.Interfaces;
using Auth.API.Services.SupportTypes;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auth.API.Services;

public class AuthenticationService
{
    private readonly AuthDbContext _dbContext;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, AuthDbContext dbContext)
    {
        _dbContext = dbContext;
        _jwtTokenGenerator = jwtTokenGenerator;
        _signInManager = signInManager;
        _userManager = userManager;

    }

    public async Task<AuthenticationResult> AuthenticateUser(ApplicationUser identityUser, DomainUser domainUser)
    {
        var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();
        
        var access = await _dbContext.Permissions.FirstOrDefaultAsync(p => p.Id == domainUser.Id);
        if (access == null)
        {
            access = new PermissionsModel(domainUser.Id, UserPermissionsPresets.User);
            await _dbContext.Permissions.AddAsync(access);
        }
        
        var token = _jwtTokenGenerator.GenerateUserToken(domainUser, access,
            DateTime.UtcNow.AddSeconds(AuthConfig.TokenLifetime));

        identityUser.AddRefreshToken(refreshToken);

        return new AuthenticationResult(token, refreshToken);
    }
    
    public async Task<AuthenticationResult> ProcessPasswordLogin(LoginType loginType, string login, string password, bool passwordRequired = true)
    {
        var identityUser = loginType switch
        {
            LoginType.Email =>
                await _userManager.FindByEmailAsync(login)
                ?? throw new NotFoundException<ApplicationUser>(),
            LoginType.Phone =>
                await _dbContext.Users.FirstOrNotFoundAsync(u => u.PhoneNumber == login)
        };
        
        var domainUser = await _dbContext.MarketUsers.FirstOrDefaultAsync(x => x.IdentityUserId == identityUser.Id)
                         ?? throw new NotFoundException<DomainUser>();

        if (passwordRequired)
        {
            var result = await _signInManager.CheckPasswordSignInAsync(identityUser, password, false);
        
            if (!result.Succeeded)
                throw new UnauthorizedException("PASSWORD_INVALID");
        }

        return await AuthenticateUser(identityUser, domainUser);
    }
    
    public async Task<AuthenticationResult> ProcessTicketLogin(string ticket)
    {
        var ticketInfo = _jwtTokenGenerator.ReadPhoneTicketRequest(ticket)
                         ?? throw new UnauthorizedException("TICKET_INVALID");

        if(ticketInfo.Scope != TicketScopes.LoginTicket)
            throw new UnauthorizedException("TICKET_SCOPE_INVALID");

        var user = ticketInfo.Type switch
        {
            TicketTypes.Email => await _userManager.Users.FirstOrDefaultAsync(x => x.Email == ticketInfo.Credentials)
                                 ?? throw new NotFoundException<ApplicationUser>(),
            TicketTypes.Phone => await _userManager.Users.FirstOrDefaultAsync(x =>
                                     x.PhoneNumber == ticketInfo.Credentials)
                                 ?? throw new NotFoundException<ApplicationUser>(),
        };
        
        var domainUser = await _dbContext.MarketUsers.FirstOrDefaultAsync(x => x.IdentityUserId == user.Id)
                         ?? throw new NotFoundException<DomainUser>();

        return await AuthenticateUser(user, domainUser);
    }
}