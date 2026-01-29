using MediatR;

namespace EventDriven.OrderProcessing.Application.Orders.Queries.GetOrders;
public sealed record GetOrdersQuery
    : IRequest<IReadOnlyList<OrderListItemDto>>;
