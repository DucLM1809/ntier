using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ntier.Business.Service;
using Ntier.Shared.Dtos;
using Ntier.Shared.Filters;
using Ntier.Shared.Models;

namespace Ntier.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly IUserService _userService;
    private readonly IValidator<CreateUserDto> _createUserValidator;
    private readonly IValidator<UpdateUserDto> _updateUserValidator;

    public UsersController(ILogger<UsersController> logger, IUserService userService, IValidator<CreateUserDto> createUserValidator, IValidator<UpdateUserDto> updateUserValidator)
    {
        _userService = userService;
        _logger = logger;
        _createUserValidator = createUserValidator;
        _updateUserValidator = updateUserValidator;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetUsers([FromQuery] QueryParameters queryParameters,
        [FromQuery] UserFilter userFilter, CancellationToken cancellationToken)
    {
        _logger.LogInformation("GetUsers API called with parameters: {QueryParameters}, Filter: {{UserFilter}}",
            queryParameters, userFilter, cancellationToken);

        var users = await _userService.GetFilteredUsersAsync(queryParameters, userFilter, cancellationToken);

        return Ok(
            new ApiResponse<List<UserResponseDto>>(
                users,
                true,
                "Users fetched successfully",
                StatusCodes.Status200OK
            )
        );
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUserById(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("GetUserById API called for ID: {Id}", id);

        var user = await _userService.GetUserByIdAsync(id, cancellationToken);

        return Ok(
            new ApiResponse<UserResponseDto>(
                user,
                true,
                "User fetched successfully",
                StatusCodes.Status200OK
            )
        );
    }

    [HttpPost("")]
    public async Task<IActionResult> AddUser([FromBody] CreateUserDto createUserDto,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("AddUser API called with data: {UserRequestDto}", createUserDto);

        var validationResult = await _createUserValidator.ValidateAsync(createUserDto, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var user = await _userService.AddUserAsync(createUserDto, cancellationToken);

        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, new ApiResponse<UserResponseDto>(
            user,
            true,
            "User created successfully",
            StatusCodes.Status201Created
        ));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("UpdateUser API called for ID: {Id} with data: {UserRequestDto}", id, updateUserDto);

        var validationResult = await _updateUserValidator.ValidateAsync(updateUserDto, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var user = await _userService.UpdateUserAsync(id, updateUserDto, cancellationToken);

        return Ok(
            new ApiResponse<UserResponseDto>(
                user,
                true,
                "User updated successfully",
                StatusCodes.Status200OK
            )
        );
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("DeleteUser API called for ID: {Id}", id);

        await _userService.DeleteUserAsync(id, cancellationToken);

        return NoContent();
    }
}