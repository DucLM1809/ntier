using Ntier.Shared.Dtos;
using Ntier.Shared.Filters;
using Ntier.Shared.Models;

namespace Ntier.Business.Service;

public interface IUserService
{
    Task<List<UserResponseDto>> GetFilteredUsersAsync(QueryParameters queryParameters, UserFilter userFilter, CancellationToken cancellationToken);
    Task<UserResponseDto> GetUserByIdAsync(int id, CancellationToken cancellationToken);
    Task<UserResponseDto> AddUserAsync(CreateUserDto userRequestDto, CancellationToken cancellationToken);
    Task<UserResponseDto> UpdateUserAsync(int id, UpdateUserDto userRequestDto, CancellationToken cancellationToken);
    Task DeleteUserAsync(int id, CancellationToken cancellationToken);
}