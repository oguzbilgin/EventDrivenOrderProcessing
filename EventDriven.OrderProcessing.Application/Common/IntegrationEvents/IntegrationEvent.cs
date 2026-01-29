namespace EventDriven.OrderProcessing.Infrastructure.Messaging.IntegrationEvents;

public abstract record IntegrationEvent(
    Guid Id,
    DateTime OccuredOn);
