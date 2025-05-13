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
        var response = new ApiResponse<object>(
            ex.Errors.Select(error => new
            {
                Field = error.PropertyName,
                Message = error.ErrorMessage
            }),
            false,
            "Validation failed",
            StatusCodes.Status400BadRequest
        );

        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(
            JsonSerializer.Serialize(
                response,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }
            )
        );
    }

    private static async Task HandleException(HttpContext context, Exception ex)
    {
        var response = new ApiResponse<object>(
            ex.Message,
            false,
            "An unexpected error occurred",
            StatusCodes.Status500InternalServerError
        );
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(
            JsonSerializer.Serialize(
                response,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }
            )
        );
    }
}