using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Auth.API.Data;
using Auth.API.Dto.ResponseDtos.Auth;
using Auth.API.Dto.ResponseDtos.Auth.External;
using Auth.API.EndpointsHandlers.Interfaces;
using Auth.API.Exceptions;
using Auth.API.InfrastructureExtensions;
using Auth.API.Models;
using Auth.API.Services;
using Auth.API.Services.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace Auth.API.EndpointsHandlers.Auth;

public class GoogleUserAuthenticationEndpointHandler : IRequestResponseEndpointHandler<string, AuthorizationResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AuthenticationService _authService;
    private readonly AuthDbContext _dbContext;
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;
    
    public GoogleUserAuthenticationEndpointHandler(UserManager<ApplicationUser> userManager, AuthenticationService authService, AuthDbContext dbContext, IMapper mapper)
    {
        _userManager = userManager;
        _authService = authService;
        _dbContext = dbContext;
        _mapper = mapper;
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://www.googleapis.com/");
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }
    
    public async Task<AuthorizationResponse> Handle(string request)
    {
        GoogleGetUserInfoResponse googleUser;
        using (_httpClient)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request);

            var response = await _httpClient.GetAsync("oauth2/v1/userinfo?alt=json");
            var responseContent = await response.Content.ReadAsStringAsync();
            
            if(!response.IsSuccessStatusCode)
                throw new BadRequestException("INCORRECT_AUTH_TOKEN");

            googleUser = JsonConvert.DeserializeObject<GoogleGetUserInfoResponse>(responseContent)!;
        }
        
        if (googleUser.Email == null || !googleUser.VerifiedEmail)
            throw new UnauthorizedException("UNVERIFIED_CREDENTIALS");

        (ApplicationUser identity, DomainUser domainUser) userDataTuple;
        
        var existingIdentity = await _userManager.FindByEmailAsync(googleUser.Email);

        if (existingIdentity is not null)
        {
            var domainEntity =
                await _dbContext.MarketUsers.FirstOrNotFoundAsync(u => u.IdentityUserId == existingIdentity.Id);
            userDataTuple = (existingIdentity, domainEntity);
        }
        else
            userDataTuple = await CreateUserFromGoogleEntity(googleUser);
        
        var tokens = await _authService.AuthenticateUser(userDataTuple.identity, userDataTuple.domainUser);
       
        await _dbContext.SaveEntitiesAsync();
        return AuthorizationResponse.FromAuthenticationResult(tokens);
        
    }

    private async Task<(ApplicationUser identity, DomainUser domainUser)> CreateUserFromGoogleEntity(GoogleGetUserInfoResponse googleUserEntity)
    {
        var identityUser = _mapper.Map<GoogleGetUserInfoResponse, ApplicationUser>(googleUserEntity);
        var domainUser = _mapper.Map<ApplicationUser, DomainUser>(identityUser);
        
        domainUser.IdentityUserId = identityUser.Id;
        domainUser.PhoneNumber = googleUserEntity.Phone;
        domainUser.Country = googleUserEntity.Locale;
        
        var googleUserName = googleUserEntity.GivenName ?? googleUserEntity.Name;
        
        domainUser.Name = googleUserName;
        identityUser.UserName = $"{Regex.Replace(TextConverter.ConvertToLatin(googleUserName), "[^a-zA-Z0-9_ ]", string.Empty)}" +
                                $"{new Random().Next(1000, 999999)}";
        
        var result = await _userManager.CreateAsync(identityUser);
        
        if (!result.Succeeded)
            throw new UnauthorizedException(string.Join("\n", result.Errors.Select(x => x.Description)));
        
        await _dbContext.MarketUsers.AddAsync(domainUser);
        
        return (identityUser, domainUser);
    }
}