using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ntier.Business.Service;
using Ntier.Shared.Dtos;
using Ntier.Shared.Models;

namespace Ntier.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetUsers([FromQuery] QueryParameters queryParameters)
    {
        var users = await _userService.GetFilteredUsersAsync(queryParameters);

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