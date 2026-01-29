using EventDriven.OrderProcessing.Application.Common.Interfaces;
using EventDriven.OrderProcessing.Domain.Orders.Events;
using EventDriven.OrderProcessing.Infrastructure.Messaging.IntegrationEvents;

namespace EventDriven.OrderProcessing.Application.Orders.EventHandlers;

public sealed class OrderCreatedDomainEventHandler
    : IDomainEventHandler<OrderCreatedDomainEvent>
{
    private readonly IIntegrationEventPublisher _publisher;

    public OrderCreatedDomainEventHandler(
        IIntegrationEventPublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task HandleAsync(
        OrderCreatedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var integrationEvent = new OrderCreatedIntegrationEvent(
            domainEvent.OrderId,
            domainEvent.TotalAmount);

        await _publisher.PublishAsync(
            integrationEvent,
            "order-created");
    }
}
