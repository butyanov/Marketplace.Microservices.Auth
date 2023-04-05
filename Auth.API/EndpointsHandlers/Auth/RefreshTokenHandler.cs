using System.Security.Claims;
using Auth.API.Data;
using Auth.API.Dto.RequestDtos;
using Auth.API.Dto.ResponseDtos;
using Auth.API.EndpointsHandlers.Interfaces;
using Auth.API.Exceptions;
using Auth.API.Models;
using Auth.API.Services;
using Auth.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auth.API.EndpointsHandlers.Auth;

public class RefreshTokenEndpointHandler : IEndpointHandler<RefreshTokenRequest, RefreshTokenResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AuthDbContext _context;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly AuthenticationService _authService;

    public RefreshTokenEndpointHandler(UserManager<ApplicationUser> userManager, AuthDbContext context, IJwtTokenGenerator jwtTokenGenerator, AuthenticationService authService)
    {
        _userManager = userManager;
        _context = context;
        _jwtTokenGenerator = jwtTokenGenerator;
        _authService = authService;
    }
    
    public async Task<RefreshTokenResponse> Handle(RefreshTokenRequest request)
    {
        var validationParamsIgnoringTime = _jwtTokenGenerator.CloneParameters();
        validationParamsIgnoringTime.ValidateLifetime = false;

        var userId =
            Guid.Parse(
                _jwtTokenGenerator.ReadToken(request.Token, validationParamsIgnoringTime)
                    .FindFirst(x => x.Type == ClaimTypes.NameIdentifier)
                    ?.Value
                ?? throw new UnauthorizedException("INVALID_OLD_TOKEN"));

        var user = await _context.MarketUsers.FirstOrDefaultAsync(x => x.Id == userId)
                   ?? throw new NotFoundException<DomainUser>();

        var applicationUser = await _userManager.FindByIdAsync(user.IdentityUserId.ToString())
                              ?? throw new NotFoundException<ApplicationUser>();
        
        var token = applicationUser.RefreshTokens.FirstOrDefault(x => x.Token == request.RefreshToken)
                    ?? throw new UnauthorizedException("INVALID_REFRESH_TOKEN");

        applicationUser.RemoveRefreshToken(token);
        
        var result = _authService.AuthenticateUser(applicationUser, user);

        await _context.SaveEntitiesAsync();
        
        return new RefreshTokenResponse(result.Token, result.RefreshToken.Token);
    }
}