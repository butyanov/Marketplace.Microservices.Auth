using Auth.API.Data.Interfaces;
using Auth.API.Dto.RequestDtos;
using Auth.API.Dto.RequestDtos.Auth;
using Auth.API.Dto.ResponseDtos;
using Auth.API.Dto.ResponseDtos.Auth;
using Auth.API.Dto.SupportTypes;
using Auth.API.Dto.SupportTypes.Auth;
using Auth.API.EndpointsHandlers.Interfaces;
using Auth.API.Exceptions;
using Auth.API.Services.Interfaces;
using Auth.API.Services.SupportTypes;

namespace Auth.API.EndpointsHandlers.Auth;

public class AcquireTicketEndpointHandler : IRequestResponseEndpointHandler<AcquireTicketRequest, AcquireTicketResponse>
{
    private readonly IRedisStore<PersistedTicketRequest> _redisStore;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AcquireTicketEndpointHandler(
        IRedisStore<PersistedTicketRequest> redisStore,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _redisStore = redisStore;
        _jwtTokenGenerator = jwtTokenGenerator;
    }
    
    public async Task<AcquireTicketResponse> Handle(AcquireTicketRequest request)
    {
        var ticket = await _redisStore.GetAsync(request.GetRedisKey());

        if (ticket is null)
            throw new NotFoundException<AcquireTicketResponse>();

        if (ticket.Code != request.Code)
            throw new ValidationFailedException("code","INVALID_CODE");
        
        await _redisStore.DeleteAsync(request.GetRedisKey());

        return new AcquireTicketResponse(
            _jwtTokenGenerator.GenerateToken(new TicketMetadata(ticket.Credentials, ticket.Scope, ticket.Type),
                DateTime.UtcNow.AddSeconds(AuthConfig.TicketLifetime)));
    }
}