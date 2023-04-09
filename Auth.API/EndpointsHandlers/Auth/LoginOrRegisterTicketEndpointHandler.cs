using Auth.API.Dto.RequestDtos;
using Auth.API.Dto.RequestDtos.Auth;
using Auth.API.Dto.ResponseDtos;
using Auth.API.Dto.ResponseDtos.Auth;
using Auth.API.EndpointsHandlers.Interfaces;
using Auth.API.Models;
using Auth.API.Services.SupportTypes;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auth.API.EndpointsHandlers.Auth;

public class LoginOrRegisterTicketEndpointHandler : IRequestResponseEndpointHandler<LoginOrRegisterTicketRequest, TicketResponse>
{
    private readonly IRequestResponseEndpointHandler<TicketRequest, TicketResponse> _ticketHandler;
    private readonly UserManager<ApplicationUser> _userManager;

    public LoginOrRegisterTicketEndpointHandler(
        IRequestResponseEndpointHandler<TicketRequest, TicketResponse> ticketHandler,
        UserManager<ApplicationUser> userManager)
    {
        _ticketHandler = ticketHandler;
        _userManager = userManager;
    }

    
    public async Task<TicketResponse> Handle(LoginOrRegisterTicketRequest request)
    {
        var exists = request.Type switch
        {
            TicketTypes.Email => await _userManager.Users.AnyAsync(x => x.Email == request.Credentials),
            TicketTypes.Phone => await _userManager.Users.AnyAsync(x => x.PhoneNumber == request.Credentials),
        };
        
        var ticketScope = exists ? TicketScopes.LoginTicket : TicketScopes.RegisterTicket;
        
        var result = await _ticketHandler.Handle(
            new TicketRequest(
                request.Credentials,
                ticketScope, request.Type));

        return new LoginOrRegisterTicketResponse(result.NextTry, result.CodeSent, exists);
    }
}