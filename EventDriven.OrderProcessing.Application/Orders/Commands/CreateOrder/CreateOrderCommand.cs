using MediatR;

namespace EventDriven.OrderProcessing.Application.Orders.Commands.CreateOrder;

public sealed record CreateOrderCommand(
    List<CreateOrderItemDto> Items
) : IRequest<Guid>;
