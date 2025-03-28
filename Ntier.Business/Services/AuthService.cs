using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Ntier.DataAccess.Repository.Interfaces;
using Ntier.Shared.Dtos;
using Ntier.Shared.Models;

namespace Ntier.Business.Service;

public class AuthService : IAuthService
{
    private readonly IJwtService _jwtService;
    private readonly ILogger<AuthService> _logger;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository, IJwtService jwtService, IMapper mapper,
        ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<string> Authenticate(string email, string password)
    {
        _logger.LogInformation("Authentication attempt for email: {Email}", email);

        try
        {
            var user = await _userRepository.GetUserByEmail(email);

            if (user == null)
            {
                _logger.LogWarning("Authentication failed for email: {Email}. User not found.", email);
                return null;
            }

            if (!VerifyPassword(password, user.Password))
            {
                _logger.LogWarning("Authentication failed for email: {Email}. Invalid password.", email);
                return null;
            }

            _logger.LogInformation("Authentication succeeded for email: {Email}. Token generated.", email);

            return _jwtService.GenerateToken(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during authentication for email: {Email}", email);
            throw;
        }
    }

    public async Task<UserResponseDto> Register(UserDto userDto)
    {
        _logger.LogInformation("User registration started for {Email}", userDto.Email);

        try
        {
            var hashedPassword = HashPassword(userDto.Password);

            var user = _mapper.Map<User>(userDto with { Password = hashedPassword });

            await _userRepository.AddAsync(user);

            _logger.LogInformation("User {Email} registered successfully with ID {UserId}", user.Email, user.Id);

            return _mapper.Map<UserResponseDto>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while registering user {Email}", userDto.Email);
            throw;
        }
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