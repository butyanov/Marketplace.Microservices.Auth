using Auth.API.Dto.RequestDtos.Auth;
using Auth.API.Dto.RequestDtos.User;
using Auth.API.Dto.ResponseDtos.User;
using AutoMapper;

namespace Auth.API.Models.TypesMapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<SignupRequest, ApplicationUser>();
        CreateMap<SignupRequest, ApplicationUser>().ReverseMap();
        CreateMap<ApplicationUser, DomainUser>();
        CreateMap<ApplicationUser, DomainUser>().ReverseMap();
        CreateMap<DomainUser, UserResponse>();
        CreateMap<UserUpdateRequest, DomainUser>();
        CreateMap<UserAdvancedUpdateRequest, DomainUser>();
        CreateMap<UserCreateRequest, DomainUser>();
    }
}