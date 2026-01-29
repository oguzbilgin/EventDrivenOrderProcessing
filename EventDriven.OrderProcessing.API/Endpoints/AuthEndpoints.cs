using EventDriven.OrderProcessing.Application.Auth.Commands.Login;
using MediatR;

namespace EventDriven.OrderProcessing.API.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("api/auth")
            .WithTags("Auth");

        group.MapPost("/login", async (
            LoginCommand command,
            IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Ok(result);
        })
        .AllowAnonymous()
        .RequireRateLimiting("login");

        return endpoints;
    }
}
