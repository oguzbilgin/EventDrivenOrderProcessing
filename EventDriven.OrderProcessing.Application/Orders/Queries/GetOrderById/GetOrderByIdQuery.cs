using MediatR;

namespace EventDriven.OrderProcessing.Application.Orders.Queries.GetOrderById;
public sealed record GetOrderByIdQuery(Guid OrderId) : IRequest<OrderDetailsDto>;
