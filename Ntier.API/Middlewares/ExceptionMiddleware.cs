using System.Text.Json;
using FluentValidation;
using Ntier.Shared.Models;

namespace Ntier.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            await HandleValidationException(context, ex);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }

    private static async Task HandleValidationException(HttpContext context, ValidationException ex)
    {
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

    private static async Task HandleException(HttpContext context, Exception ex)
    {
        var result = Result.FailureResult(
            "An unexpected error occurred",
            ex.Message,
            StatusCodes.Status500InternalServerError
        );

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
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