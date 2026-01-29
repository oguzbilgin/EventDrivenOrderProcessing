using EventDriven.OrderProcessing.Application.Common.Interfaces;
using MediatR;

namespace EventDriven.OrderProcessing.Application.Orders.Commands.StartPayment;

public sealed class StartPaymentCommandHandler
    : IRequestHandler<StartPaymentCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICurrentUser _currentUser;

    public StartPaymentCommandHandler(
        IOrderRepository orderRepository,
        ICurrentUser currentUser)
    {
        _orderRepository = orderRepository;
        _currentUser = currentUser;
    }

    public async Task Handle(
        StartPaymentCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository
            .GetIdByAsync(request.OrderId, cancellationToken);

        if (order is null)
            throw new InvalidOperationException("Order not found.");

        if (order.UserId != _currentUser.UserId)
            throw new InvalidOperationException("Forbidden");

        order.StartPayment();

        await _orderRepository.SaveChangesAsync(cancellationToken);
    }
}
