using AutoMapper;
using Ntier.DataAccess.Repository.Interfaces;
using Ntier.Shared.Dtos;
using Ntier.Shared.Models;

namespace Ntier.Business.Service;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository, IJwtService jwtService, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<List<UserResponseDto>> GetFilteredUsersAsync(QueryParameters queryParameters)
    {
        var users = await _userRepository.GetFilteredAsync(null, queryParameters);

        return _mapper.Map<List<UserResponseDto>>(users);
    }

    public Task<List<UserResponseDto>> GetFilteredUsersAsync()
    {
        throw new NotImplementedException();
    }
}