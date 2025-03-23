using AutoMapper;
using Ntier.Shared.Dtos;
using Ntier.Shared.Enums;
using Ntier.Shared.Models;

namespace Ntier.Business.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        // Map Enum Role to int
        CreateMap<UserDto, User>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => (int)src.Role));

        // Map int Role to Role enum
        CreateMap<User, UserResponseDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => (Role)src.Role));
    }
}