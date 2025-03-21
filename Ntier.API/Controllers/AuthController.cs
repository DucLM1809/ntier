using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Ntier.Business.Service;
using Ntier.Shared.Dtos;
using Ntier.Shared.Models;

namespace Ntier.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IValidator<UserDto> _validator;

    public AuthController(IAuthService authService, IValidator<UserDto> validator)
    {
        (_authService, _validator) = (authService, validator);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDto request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var user = await _authService.Register(request.Email, request.Password, request.Role.ToString());

        return Ok(
            new ApiResponse<UserResponseDto>(
                user,
                true,
                "User registered successfully",
                StatusCodes.Status201Created
            )
        );
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserDto request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var token = await _authService.Authenticate(request.Email, request.Password);
        if (token == null)
            return Unauthorized();

        return Ok(
            new ApiResponse<string>(
                token,
                true,
                "User authenticated successfully",
                StatusCodes.Status200OK
            )
        );
    }
}