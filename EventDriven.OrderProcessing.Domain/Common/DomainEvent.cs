namespace EventDriven.OrderProcessing.Domain.Common;
public abstract class DomainEvent
{
    public DateTime OccuredOn { get; } = DateTime.UtcNow;
}