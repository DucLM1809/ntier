using AutoMapper;
using Ntier.Shared.Dtos;
using Ntier.Shared.Models;

namespace Ntier.Business.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserResponseDto>();
        }
    }
}
