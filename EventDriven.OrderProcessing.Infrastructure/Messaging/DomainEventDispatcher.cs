using EventDriven.OrderProcessing.Application.Common.Interfaces;
using EventDriven.OrderProcessing.Domain.Common;
using Microsoft.Extensions.DependencyInjection;

namespace EventDriven.OrderProcessing.Infrastructure.Messaging;

public sealed class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public DomainEventDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task DispatchAsync(
        IEnumerable<DomainEvent> domainEvents,
        CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in domainEvents)
        {
            var handlerType = typeof(IDomainEventHandler<>)
                .MakeGenericType(domainEvent.GetType());

            var handlers = _serviceProvider.GetServices(handlerType);

            foreach (var handler in handlers)
            {
                var method = handlerType.GetMethod("HandleAsync")!;

                await (Task)method.Invoke(
                    handler,
                    new object[] { domainEvent, cancellationToken })!;
            }
        }
    }
}
