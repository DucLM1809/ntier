using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Ntier.Business.Service;
using Ntier.Shared.Dtos;
using Ntier.Shared.Models;

namespace Ntier.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IValidator<LoginDto> _loginValidator;
    private readonly IValidator<UserDto> _userValidator;

    public AuthController(IAuthService authService, IValidator<UserDto> userValidator,
        IValidator<LoginDto> loginValidator)
    {
        (_authService, _userValidator, _loginValidator) = (authService, userValidator, loginValidator);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDto request)
    {
        var validationResult = await _userValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var user = await _authService.Register(request);

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
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        var validationResult = await _loginValidator.ValidateAsync(request);
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