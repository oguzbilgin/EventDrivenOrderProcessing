using System.Text.Json;
using EventDriven.OrderProcessing.Domain.Exceptions;
using FluentValidation;

namespace EventDriven.OrderProcessing.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);

            if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                await HandleUnauthorized(context);
            }
            else if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                await HandleForbidden(context);
            }
        }
        catch (ValidationException ex)
        {
            await HandleValidationException(context, ex);
        }
        catch (DomainException ex)
        {
            await HandleDomainException(context, ex);
        }
        catch (Exception)
        {
            await HandleUnhandledException(context);
        }
    }

    private async Task HandleUnauthorized(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        var response = new { type = "Unauthorized", message = "Authentication required." };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private async Task HandleForbidden(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        var response = new { type = "Forbidden", message = "You do not have permission." };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private async Task HandleUnhandledException(HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        var response = new
        {
            type = "ServerError",
            message = "An unexpected error occurred."
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response)
        );
    }

    private async Task HandleDomainException(HttpContext context, DomainException ex)
    {
        context.Response.StatusCode = ex is GenericDomainException gde && gde.Code == "InvalidCredentials"
            ? StatusCodes.Status401Unauthorized
            : StatusCodes.Status409Conflict;

        context.Response.ContentType = "application/json";

        var response = new
        {
            type = "DomainError",
            message = ex.Message
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response)
        );
    }

    private async Task HandleValidationException(HttpContext context, ValidationException ex)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.ContentType = "application/json";

        var errors = ex.Errors
            .GroupBy(x => x.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.ErrorMessage).ToArray()
            );

        var response = new
        {
            type = "ValidationError",
            errors
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response)
        );
    }
}
