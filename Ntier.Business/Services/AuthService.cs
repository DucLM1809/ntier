using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Ntier.DataAccess.Repositories.Interfaces;
using Ntier.Shared.Dtos;
using Ntier.Shared.Models;

namespace Ntier.Business.Service;

public class AuthService : IAuthService
{
    private readonly IJwtService _jwtService;
    private readonly ILogger<AuthService> _logger;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(ILogger<AuthService> logger, IJwtService jwtService, IMapper mapper,
        IUnitOfWork unitOfWork
    )
    {
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<UserResponseDto> Register(RegisterDto registerDto, CancellationToken cancellationToken)
    {
        _logger.LogInformation("User registration started for {Email}", registerDto.Email);

        try
        {
            var hashedPassword = HashPassword(registerDto.Password);

            var user = _mapper.Map<User>(registerDto with { Password = hashedPassword });

            await _unitOfWork.Users.AddAsync(user, cancellationToken);

            _logger.LogInformation("User {Email} registered successfully with ID {UserId}", user.Email, user.Id);

            return _mapper.Map<UserResponseDto>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while registering user {Email}", registerDto.Email);
            throw;
        }
    }

    public async Task<AuthResponseDto> Authenticate(LoginDto loginDto, string? deviceInfo, string? ipAddress, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Authentication attempt for email: {Email}", loginDto.Email);

        try
        {
            var user = await _unitOfWork.Users.GetUserByEmail(loginDto.Email, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("Authentication failed for email: {Email}. User not found.", loginDto.Email);
                return null;
            }

            if (!VerifyPassword(loginDto.Password, user.Password))
            {
                _logger.LogWarning("Authentication failed for email: {Email}. Invalid password.", loginDto.Email);
                return null;
            }


            var accessToken = _jwtService.GenerateToken(user);
            var refreshToken = GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                ExpiryDate = DateTimeOffset.UtcNow.AddDays(7),
                IsRevoked = false,
                DeviceInfo = deviceInfo,
                IpAddress = ipAddress,
                UserId = user.Id
            };

            await _unitOfWork.RefreshTokens.AddAsync(refreshTokenEntity, cancellationToken);

            _logger.LogInformation("Authentication succeeded for email: {Email}. Token generated.", loginDto.Email);


            return new AuthResponseDto(accessToken, refreshToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during authentication for email: {Email}", loginDto.Email);
            throw;
        }
    }

    public async Task<AuthResponseDto> RefreshToken(RefreshTokenDto refreshTokenDto, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Attempting to refresh token for {RefreshToken}", refreshTokenDto.RefreshToken);

        try
        {
            var storedToken = await _unitOfWork.RefreshTokens.GetByTokenAsync(refreshTokenDto.RefreshToken, cancellationToken);
            if (storedToken == null)
            {
                _logger.LogWarning("Refresh token not found: {RefreshToken}", refreshTokenDto.RefreshToken);
                return null;
            }

            if (storedToken.IsRevoked)
            {
                _logger.LogWarning("Refresh token is revoked: {RefreshToken}", refreshTokenDto.RefreshToken);
                return null;
            }

            if (storedToken.ExpiryDate < DateTime.UtcNow)
            {
                _logger.LogWarning("Refresh token is expired: {RefreshToken}, Expiry: {ExpiryDate}",
                    refreshTokenDto.RefreshToken, storedToken.ExpiryDate);
                return null;
            }

            var user = storedToken.User;
            var newAccessToken = _jwtService.GenerateToken(user);
            var newRefreshToken = GenerateRefreshToken();

            storedToken.Token = newRefreshToken;
            storedToken.ExpiryDate = DateTime.UtcNow.AddDays(7);
            storedToken.IsRevoked = false;
            await _unitOfWork.RefreshTokens.UpdateAsync(storedToken, cancellationToken);

            _logger.LogInformation("Refresh token successfully generated for user {UserId}", user.Id);

            return new AuthResponseDto(newAccessToken, newRefreshToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while refreshing the token for {RefreshToken}",
                refreshTokenDto.RefreshToken);
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

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}