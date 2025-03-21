using Ntier.Shared.Dtos;

namespace Ntier.Business.Service;

public interface IAuthService
{
    Task<string> Authenticate(string username, string password);
    Task<UserResponseDto> Register(string username, string password, string role);
}