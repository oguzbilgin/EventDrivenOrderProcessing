namespace EventDriven.OrderProcessing.Application.Common.Interfaces;

public interface IIntegrationEventPublisher
{
    Task PublishAsync<T>(T @event, string queueName);
}
