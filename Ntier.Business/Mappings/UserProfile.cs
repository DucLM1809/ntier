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

        // Map CreateUserDto to User
        CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => (int)Role.User))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => (int)src.Gender));

        // Map UpdateUserDto to User
        CreateMap<UpdateUserDto, User>()
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => (int)src.Gender))
            .ForMember(dest => dest.Email, opt => opt.Ignore())
            .ForMember(dest => dest.Password, opt => opt.Ignore());
    }
}