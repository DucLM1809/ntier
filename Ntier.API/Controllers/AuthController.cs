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
    private readonly ILogger<AuthController> _logger;
    private readonly IValidator<LoginDto> _loginValidator;
    private readonly IValidator<UserDto> _userValidator;

    public AuthController(ILogger<AuthController> logger, IAuthService authService, IValidator<UserDto> userValidator,
        IValidator<LoginDto> loginValidator)
    {
        _logger = logger;
        _authService = authService;
        _userValidator = userValidator;
        _loginValidator = loginValidator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDto request)
    {
        _logger.LogInformation("Register API called for {Email}", request.Email);

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
        _logger.LogInformation("Login API called for {Email}", request.Email);

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