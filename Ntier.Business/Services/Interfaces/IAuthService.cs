using Ntier.Shared.Dtos;

namespace Ntier.Business.Service;

public interface IAuthService
{
    Task<AuthResponseDto> Authenticate(LoginDto loginDto, string? deviceInfo, string? ipAddress, CancellationToken cancellationToken);
    Task<AuthResponseDto> RefreshToken(RefreshTokenDto refreshToken, CancellationToken cancellationToken);
    Task<UserResponseDto> Register(RegisterDto userDto, CancellationToken cancellationToken);
}