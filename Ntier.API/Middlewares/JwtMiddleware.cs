using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using Ntier.Business.Exceptions;
using Ntier.Shared.Models;

namespace Ntier.API.Middlewares;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _secret;
    private readonly ILogger<JwtMiddleware> _logger;

    public JwtMiddleware(RequestDelegate next, string secret, ILogger<JwtMiddleware> logger)
    {
        _next = next;
        _secret = secret;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        if (!string.IsNullOrEmpty(token))
        {
            try
            {
                _logger.LogInformation("Validating JWT token: {Token}", token);

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_secret);
                var parameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true
                };

                var principal = tokenHandler.ValidateToken(token, parameters, out var validatedToken);
                context.User = principal;
            }
            catch (Exception ex)
            {
                throw new UnauthorizedException(ex.Message);
            }
        }

        await _next(context);
    }
}