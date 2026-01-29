using EventDriven.OrderProcessing.Domain.Common;

namespace EventDriven.OrderProcessing.Application.Common.Interfaces;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IEnumerable<DomainEvent> domainEvents, CancellationToken cancellationToken);
}
