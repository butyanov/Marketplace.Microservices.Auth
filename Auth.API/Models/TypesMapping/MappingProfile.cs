using Auth.API.Dto.RequestDtos;
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
    }
}