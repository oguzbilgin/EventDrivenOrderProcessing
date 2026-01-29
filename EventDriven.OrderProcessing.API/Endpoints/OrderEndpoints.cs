using EventDriven.OrderProcessing.Application.Orders.Commands.CreateOrder;
using EventDriven.OrderProcessing.Application.Orders.Commands.StartPayment;
using EventDriven.OrderProcessing.Application.Orders.Queries.GetOrderById;
using EventDriven.OrderProcessing.Application.Orders.Queries.GetOrders;
using MediatR;

namespace EventDriven.OrderProcessing.API.Endpoints;

public static class OrderEndpoints
{
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("api/orders")
            .RequireAuthorization("AdminOnly")
            .WithTags("Orders");

        group.MapGet("/{id:guid}", async (Guid id, IMediator mediator) =>
        {
            var order = await mediator.Send(new GetOrderByIdQuery(id));

            return order is null
                ? Results.NotFound()
                : Results.Ok(order);
        });

        group.MapPost("/", async (CreateOrderCommand command, IMediator mediator) =>
        {
            var id = await mediator.Send(command);
            return Results.Created($"/api/orders/{id}", null);
        });

        group.MapPost("/{id:guid}/start-payment", async (
            Guid id, IMediator mediator) =>
        {
            await mediator.Send(new StartPaymentCommand(id));
            return Results.NoContent();
        });

        return endpoints;
    }
}
