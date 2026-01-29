using EventDriven.OrderProcessing.Domain.Common;

namespace EventDriven.OrderProcessing.Domain.Payments.Events;

public sealed class PaymentFailedDomainEvent : DomainEvent
{
    public Guid OrderId { get; }
    public PaymentFailedDomainEvent(Guid orderId)
    {
        OrderId = orderId;   
    }
}
