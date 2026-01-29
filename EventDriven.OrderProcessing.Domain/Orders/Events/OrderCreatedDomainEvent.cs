
using EventDriven.OrderProcessing.Domain.Common;

namespace EventDriven.OrderProcessing.Domain.Orders.Events; 

public sealed class OrderCreatedDomainEvent : DomainEvent
{
    public Guid OrderId { get; }
    public decimal TotalAmount { get; }

    public OrderCreatedDomainEvent(Guid orderId, decimal totalAmount)
    {
        OrderId = orderId;
        TotalAmount = totalAmount;
    }
}
