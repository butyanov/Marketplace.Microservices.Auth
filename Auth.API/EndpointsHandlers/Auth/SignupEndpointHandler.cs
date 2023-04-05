using Auth.API.Data.Interfaces;
using Auth.API.Dto.RequestDtos;
using Auth.API.Dto.ResponseDtos;
using Auth.API.Dto.SupportTypes;
using Auth.API.EndpointsHandlers.Interfaces;
using Auth.API.Exceptions;
using Auth.API.Models;
using Auth.API.Services;
using Auth.API.Services.Extensions;
using Auth.API.Services.Interfaces;
using Auth.API.Services.SupportTypes;
using Auth.API.Services.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Auth.API.EndpointsHandlers.Auth;

public class SignupEndpointHandler : IEndpointHandler<SignupRequest, AuthorizationResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IDomainDbContext _dbContext;
    private readonly AuthenticationService _authService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IMapper _mapper;
    
    public SignupEndpointHandler(UserManager<ApplicationUser> userManager, AuthenticationService authService, IMapper mapper, IDomainDbContext dbContext, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userManager = userManager;
        _authService = authService;
        _mapper = mapper;
        _dbContext = dbContext;
        _jwtTokenGenerator = jwtTokenGenerator;
    }
    
    public async Task<AuthorizationResponse> Handle(SignupRequest request)
    {
        var ticket = _jwtTokenGenerator.ReadPhoneTicketRequest(request.Ticket)
                     ?? throw new UnauthorizedException("TICKET_INVALID");
        
        if(ticket.Scope != TicketScopes.RegisterTicket)
            throw new UnauthorizedException("TICKET_SCOPE_INVALID");

        switch (ticket.Type)
        {
            case TicketTypes.Email:
                if(_userManager.Users.Any(u => u.Email == ticket.Credentials))
                    throw new AlreadyExistsException("User");
                break;
            case TicketTypes.Phone:
                if(_userManager.Users.Any(u => u.PhoneNumber == ticket.Credentials))
                    throw new AlreadyExistsException("User");
                break;
        }
        
        var (identity, domainUser) = await CreateUser(request, ticket);
        var tokens = _authService.AuthenticateUser(identity, domainUser);
        await _dbContext.SaveEntitiesAsync();
        return AuthorizationResponse.FromAuthenticationResult(tokens);
    }
    
    private async Task<(ApplicationUser identity, DomainUser domainUser)> CreateUser(
        SignupRequest request, TicketMetadata ticket)
    {
        var identityUser = _mapper.Map<SignupRequest, ApplicationUser>(request);
        var domainUser = _mapper.Map<ApplicationUser, DomainUser>(identityUser);
        domainUser.IdentityUserId = identityUser.Id;
        domainUser.Name = request.Name;
        identityUser.UserName = $"{TextConverter.ConvertToLatin(domainUser.Name.Replace(" ", string.Empty))}{new Random().Next(1000, 999999)}";
        
        switch (ticket.Type)
        {
            case TicketTypes.Email:
                domainUser.Email = identityUser.Email = ticket.Credentials;
                identityUser.EmailConfirmed = true;
                break;
            case TicketTypes.Phone:
                domainUser.PhoneNumber = identityUser.PhoneNumber = ticket.Credentials;
                identityUser.PhoneNumberConfirmed = true;
                break;
        }
        
        var result = await _userManager.CreateAsync(identityUser, request.Password);
        await _dbContext.MarketUsers.AddAsync(domainUser);

        if (!result.Succeeded)
            throw new UnauthorizedException(string.Join("\n", result.Errors.Select(x => x.Description)));
        
        return (identityUser, domainUser);
    }
}
