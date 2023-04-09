using System.Security.Claims;
using Auth.API.Data;
using Auth.API.Dto.ResponseDtos.User;
using Auth.API.EndpointsHandlers.Interfaces;
using Auth.API.Exceptions;
using Auth.API.InfrastructureExtensions;
using Auth.API.Models;
using Auth.API.Services.Interfaces;
using AutoMapper;

namespace Auth.API.EndpointsHandlers.Me;

public class MeGetEndpointHandler : IResponseEndpointHandler<UserResponse>
{
    private readonly string _token;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly AuthDbContext _dbContext;
    private readonly IMapper _mapper;
    public MeGetEndpointHandler(IHttpContextAccessor httpContextAccessor, IJwtTokenGenerator jwtTokenGenerator, AuthDbContext dbContext, IMapper mapper)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _dbContext = dbContext;
        _mapper = mapper;
        _token = httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
    }
    
    public async Task<UserResponse> Handle()
    {
        var userId =
            Guid.Parse(
                _jwtTokenGenerator.ReadToken(_token)
                    .FindFirst(x => x.Type == ClaimTypes.NameIdentifier)
                    ?.Value
                ?? throw new UnauthorizedException("INVALID_TOKEN"));
        var user = await _dbContext.MarketUsers.FirstOrNotFoundAsync(u => u.Id == userId);
        return _mapper.Map<DomainUser, UserResponse>(user);
    }
}