using EventDriven.OrderProcessing.Domain.Common;

namespace EventDriven.OrderProcessing.Domain.Payments.Events;

public sealed class PaymentSucceededDomainEvent : DomainEvent
{
    public Guid OrderId { get; }
    public PaymentSucceededDomainEvent(Guid orderId)
    {
        OrderId = orderId;
    }
}
