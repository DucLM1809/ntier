using Ntier.Shared.Dtos;

namespace Ntier.Business.Service;

public interface IAuthService
{
    Task<AuthResponseDto> Authenticate(LoginDto loginDto, string? deviceInfo, string? ipAddress);
    Task<AuthResponseDto> RefreshToken(RefreshTokenDto refreshToken);
    Task<UserResponseDto> Register(RegisterDto userDto);
}