using AutoMapper;
using Microsoft.Extensions.Logging;
using Ntier.Business.Service.Extensions;
using Ntier.DataAccess.Repositories.Interfaces;
using Ntier.Shared.Dtos;
using Ntier.Shared.Filters;
using Ntier.Shared.Models;

namespace Ntier.Business.Service;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(ILogger<UserService> logger, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<UserResponseDto>> GetFilteredUsersAsync(QueryParameters queryParameters,
        UserFilter userFilter, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching filtered users with parameters: {QueryParameters}, Filter: {{UserFilter}}",
            queryParameters, userFilter);

        try
        {
            var filterExpression = UserFilteringExtensions.BuildFilter(userFilter);

            var users = await _unitOfWork.Users.GetFilteredAsync(filterExpression, queryParameters, cancellationToken);

            _logger.LogInformation("Successfully retrieved {UserCount} users.", users.Count);

            return _mapper.Map<List<UserResponseDto>>(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching filtered users with parameters: {QueryParameters}",
                queryParameters);
            throw;
        }
    }
}