using EventDriven.OrderProcessing.Domain.Common;
using EventDriven.OrderProcessing.Domain.Orders.Events;
using EventDriven.OrderProcessing.Domain.Users;

namespace EventDriven.OrderProcessing.Domain.Orders;
public sealed class Order : BaseEntity
{
    private readonly List<OrderItem> _items = new();

    public Guid UserId { get; private set; }
    public User User { get; private set; } = default!;
    public OrderStatus Status { get; private set; }
    public decimal TotalAmount { get; private set; }

    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    private Order() { } // EF

    public Order(Guid userId)
    {
        UserId = userId;
        Status = OrderStatus.Pending;
    }

    public void AddItem(string productName, decimal price, int quantity)
    {
        if (Status != OrderStatus.Pending)
        {
            throw new InvalidOperationException("Cannot modify order after payment started.");
        }

        _items.Add(new OrderItem(Id, productName, price, quantity));
        RecalculateTotal();
    }

    public void StartPayment()
    {
        if (!_items.Any())
        {
            throw new InvalidOperationException("Order must have at least one item.");
        }

        Status = OrderStatus.PaymentPending;

        AddDomainEvent(new OrderCreatedDomainEvent(Id, TotalAmount));
    }

    public void MarkAsPaid()
    {
        Status = OrderStatus.Paid;
    }

    public void MarkAsPaymentFailed()
    {
        Status = OrderStatus.PaymentFailed;
    }

    public void Complete()
    {
        if (Status != OrderStatus.Paid)
        {
            throw new InvalidOperationException("Only paid orders can be completed.");
        }

        Status = OrderStatus.Completed;
    }

    private void RecalculateTotal()
    {
        TotalAmount = _items.Sum(i => i.Price * i.Quantity);
    }
}
