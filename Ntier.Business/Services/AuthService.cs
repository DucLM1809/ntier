using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Ntier.DataAccess.Repository.Interfaces;
using Ntier.Shared.Dtos;
using Ntier.Shared.Models;

namespace Ntier.Business.Service;

public class AuthService : IAuthService
{
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository, IJwtService jwtService, IMapper mapper)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _mapper = mapper;
    }

    public async Task<string> Authenticate(string email, string password)
    {
        var user = await _userRepository.GetUserByEmail(email);
        if (user == null || !VerifyPassword(password, user.Password)) return null;

        return _jwtService.GenerateToken(user);
    }

    public async Task<UserResponseDto> Register(UserDto userDto)
    {
        var hashedPassword = HashPassword(userDto.Password);

        var user = _mapper.Map<User>(userDto with { Password = hashedPassword });

        await _userRepository.AddAsync(user);

        return _mapper.Map<UserResponseDto>(user);
    }

    private string HashPassword(string password)
    {
        var sha256 = SHA256.Create();
        return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
    }

    private bool VerifyPassword(string password, string hashedPassword)
    {
        return HashPassword(password) == hashedPassword;
    }
}