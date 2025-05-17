using AutoMapper;
using Microsoft.Extensions.Logging;
using Ntier.Business.Exceptions;
using Ntier.Business.Service.Extensions;
using Ntier.Business.Utils;
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

    public async Task<UserResponseDto> GetUserByIdAsync(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching user with ID: {UserId}", id);

        try
        {
            var user = await _unitOfWork.Users.GetUserByIdAsync(id, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("User with ID: {UserId} not found.", id);
                throw new NotFoundException($"User with ID: {id} not found.");
            }

            return _mapper.Map<UserResponseDto>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching user with ID: {UserId}", id);
            throw;
        }
    }

    public async Task<UserResponseDto> AddUserAsync(CreateUserDto userRequestDto, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Adding new user with request: {UserRequestDto}", userRequestDto);

        try
        {
            var user = _mapper.Map<CreateUserDto, User>(userRequestDto);

            user.Password = PasswordHelper.HashPassword("Aqswde123@");

            var createdUser = await _unitOfWork.Users.AddUser(user, cancellationToken);

            _logger.LogInformation("User added successfully with ID: {UserId}", createdUser.Id);

            return _mapper.Map<UserResponseDto>(createdUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding a new user with request: {UserRequestDto}",
                userRequestDto);
            throw;
        }
    }

    public Task DeleteUserAsync(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting user with ID: {UserId}", id);

        try
        {
            return _unitOfWork.Users.DeleteUser(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting user with ID: {UserId}", id);
            throw;
        }
    }


    public async Task<UserResponseDto> UpdateUserAsync(int id, UpdateUserDto updateUserDto,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating user with ID: {UserId} and request: {UserRequestDto}", id, updateUserDto);

        try
        {
            var existingUser = await _unitOfWork.Users.GetUserByIdAsync(id, cancellationToken);

            var user = _mapper.Map<UpdateUserDto, User>(updateUserDto);
            user.Id = id;
            user.Password = existingUser.Password;
            user.Email = existingUser.Email;
            user.Role = existingUser.Role;

            var updatedUser = await _unitOfWork.Users.UpdateUser(user, cancellationToken);

            _logger.LogInformation("User with ID: {UserId} updated successfully.", id);

            return _mapper.Map<UserResponseDto>(updatedUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "An error occurred while updating user with ID: {UserId} and request: {UserRequestDto}",
                id, updateUserDto);
            throw;
        }
    }
}