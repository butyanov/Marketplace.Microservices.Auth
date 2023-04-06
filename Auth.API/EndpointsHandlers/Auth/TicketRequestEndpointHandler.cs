using Auth.API.Data.Interfaces;
using Auth.API.Dto.RequestDtos;
using Auth.API.Dto.ResponseDtos;
using Auth.API.EndpointsHandlers.Interfaces;
using Auth.API.Models;
using Auth.API.Services.Interfaces;
using Auth.API.Services.SupportTypes;
using Microsoft.AspNetCore.Identity;

namespace Auth.API.EndpointsHandlers.Auth;

public class TicketRequestEndpointHandler : IEndpointHandler<TicketRequest, TicketResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IVerificationCodeGeneratorService _verificationCodeGeneratorService;
    private readonly ISenderService _senderService;
    private readonly IRedisStore<PersistedTicketRequest> _redisStore;

    public TicketRequestEndpointHandler(UserManager<ApplicationUser> userManager, IVerificationCodeGeneratorService verificationService, IRedisStore<PersistedTicketRequest> redisStore, ISenderService senderService)
    {
        _userManager = userManager;
        _verificationCodeGeneratorService = verificationService;
        _senderService = senderService;
        _redisStore = redisStore;
    }


    public async Task<TicketResponse> Handle(TicketRequest request)
    {
        var key = request.GetRedisKey();
        var oldTicket = await _redisStore.GetAsync(key);

        if (oldTicket != null && (oldTicket.NextTry - DateTime.UtcNow) > TimeSpan.Zero)
            return new TicketResponse(oldTicket.NextTry, false);

        var code = _verificationCodeGeneratorService.GetVerificationCode(request.Credentials);
        if (request.Type == TicketTypes.Email)
            await _senderService.SendAsync(request.Credentials, code);

        var expiresAt = TimeSpan.FromSeconds(AuthConfig.TicketRequestLifetime);
        var nextRequestAt = DateTime.UtcNow.AddSeconds(AuthConfig.TicketCooldown);

        await _redisStore.SetAsync(key,
            new PersistedTicketRequest(request.Credentials, code, nextRequestAt, request.Scope, request.Type),
            expiresAt);

        return new TicketResponse(nextRequestAt, true);
    }
}