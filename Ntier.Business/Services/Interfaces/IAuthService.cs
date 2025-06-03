using Ntier.Shared.Dtos;

namespace Ntier.Business.Service;

public interface IAuthService
{
    Task<AuthResponseDto> Authenticate(LoginDto loginDto, string? deviceInfo, string? ipAddress,
        CancellationToken cancellationToken = default);

    Task<AuthResponseDto> RefreshToken(RefreshTokenDto refreshToken, CancellationToken cancellationToken = default);
    Task<UserResponseDto> Register(RegisterDto userDto, CancellationToken cancellationToken = default);
}