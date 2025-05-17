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
    private readonly ILogger<AuthController> _logger;
    private readonly IValidator<LoginDto> _loginValidator;
    private readonly IValidator<RegisterDto> _registerValidator;

    public AuthController(ILogger<AuthController> logger, IAuthService authService,
        IValidator<RegisterDto> registerValidator,
        IValidator<LoginDto> loginValidator)
    {
        _logger = logger;
        _authService = authService;
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Register API called for {Email}", request.Email);

        var validationResult = await _registerValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var user = await _authService.Register(request, cancellationToken);

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
    public async Task<IActionResult> Login([FromBody] LoginDto request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Login API called for {Email}", request.Email);

        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        var deviceInfo = Request.Headers["User-Agent"].ToString();

        var validationResult = await _loginValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var token = await _authService.Authenticate(request, deviceInfo, ipAddress, cancellationToken);
        if (token == null)
            return Unauthorized();

        return Ok(
            new ApiResponse<AuthResponseDto>(
                token,
                true,
                "User authenticated successfully",
                StatusCodes.Status200OK
            )
        );
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto refreshTokenDto, CancellationToken cancellationToken)
    {
        var response = await _authService.RefreshToken(refreshTokenDto, cancellationToken);
        if (response == null)
            return Unauthorized(new { message = "Invalid refresh token" });

        return Ok(new ApiResponse<AuthResponseDto>(response, true, "Token refreshed successfully",
            StatusCodes.Status200OK));
    }
}