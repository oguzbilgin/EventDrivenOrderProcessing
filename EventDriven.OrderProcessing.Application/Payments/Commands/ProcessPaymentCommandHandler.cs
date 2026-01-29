using EventDriven.OrderProcessing.Application.Common.Interfaces;
using MediatR;

namespace EventDriven.OrderProcessing.Application.Payments.Commands;

public sealed class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand>
{
    private readonly IOrderRepository _orderRepository;

    public ProcessPaymentCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;   
    }

    public async Task Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetIdByAsync(request.OrderId, cancellationToken);

        if (order is null)
        {
            throw new InvalidOperationException("Order not found.");
        }

        //Simulation for testing on development, should be replaced with Payment service on prod
        var success = Random.Shared.Next(0, 100) >= 20;

        if (success)
        {
            order.MarkAsPaid();
            order.Complete();
            await _orderRepository.SaveChangesAsync(cancellationToken);
        } else
        {
            order.MarkAsPaymentFailed();
        }
    }
}
