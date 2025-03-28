using Ntier.Shared.Dtos;
using Ntier.Shared.Filters;
using Ntier.Shared.Models;

namespace Ntier.Business.Service;

public interface IUserService
{
    Task<List<UserResponseDto>> GetFilteredUsersAsync(QueryParameters queryParameters, UserFilter userFilter);
}