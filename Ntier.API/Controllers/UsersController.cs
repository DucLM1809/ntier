using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ntier.Business.Service;
using Ntier.Shared.Dtos;
using Ntier.Shared.Filters;
using Ntier.Shared.Models;

namespace Ntier.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly IUserService _userService;

    public UsersController(ILogger<UsersController> logger, IUserService userService)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetUsers([FromQuery] QueryParameters queryParameters,
        [FromQuery] UserFilter userFilter)
    {
        _logger.LogInformation("GetUsers API called with parameters: {QueryParameters}, Filter: {{UserFilter}}",
            queryParameters, userFilter);

        var users = await _userService.GetFilteredUsersAsync(queryParameters, userFilter);

        return Ok(
            new ApiResponse<List<UserResponseDto>>(
                users,
                true,
                "Users fetched successfully",
                StatusCodes.Status200OK
            )
        );
    }
}