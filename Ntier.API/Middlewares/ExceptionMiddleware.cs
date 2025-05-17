using System.Text.Json;
using FluentValidation;
using Ntier.Business.Exceptions;
using Ntier.Shared.Models;

namespace Ntier.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            await HandleValidationException(context, ex, _logger);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex, _logger);
        }
    }

    private static async Task HandleValidationException(HttpContext context, ValidationException ex, ILogger<ExceptionMiddleware> logger)
    {
        // Log the validation exception (you can use a logging framework like Serilog, NLog, etc.)
        logger.LogError(ex, "Validation error occurred.");

        // Create a result object to return
        var result = Result.FailureResult(
            ex.Message,
            ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }),
            StatusCodes.Status400BadRequest
        );

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(
                result,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }
            )
        );
    }

    private static async Task HandleException(HttpContext context, Exception ex, ILogger<ExceptionMiddleware> logger)
    {
        // Log the exception (you can use a logging framework like Serilog, NLog, etc.)
        logger.LogError(ex, "An unhandled exception occurred.");


        var code = ex switch
        {
            // Handle specific exceptions here and set the status code accordingly
            BadRequestException => StatusCodes.Status400BadRequest,
            UnauthorizedException => StatusCodes.Status401Unauthorized,
            NotFoundException => StatusCodes.Status404NotFound,
            UnprocessableRequestException => StatusCodes.Status422UnprocessableEntity,
            _ => StatusCodes.Status500InternalServerError
        };

        var result = Result.FailureResult(
            "An unexpected error occurred",
            ex.Message,
            code
        );

        context.Response.StatusCode = code;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(
                result,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }
            )
        );
    }
}