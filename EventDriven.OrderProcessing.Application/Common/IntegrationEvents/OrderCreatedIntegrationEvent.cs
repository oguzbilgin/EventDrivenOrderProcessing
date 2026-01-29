namespace EventDriven.OrderProcessing.Infrastructure.Messaging.IntegrationEvents;

public sealed record OrderCreatedIntegrationEvent(
    Guid OrderId,
    decimal TotalAmount
) : IntegrationEvent(Guid.NewGuid(), DateTime.UtcNow);
