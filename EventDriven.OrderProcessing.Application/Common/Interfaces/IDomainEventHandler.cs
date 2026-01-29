using EventDriven.OrderProcessing.Domain.Common;

namespace EventDriven.OrderProcessing.Application.Common.Interfaces;

public interface IDomainEventHandler<in TEvent>
    where TEvent : DomainEvent
{
    Task HandleAsync(TEvent domainEvent, CancellationToken cancellationToken);
}
