using EventDriven.OrderProcessing.Domain.Common;

namespace EventDriven.OrderProcessing.Domain.Orders;

public sealed class OrderItem : BaseEntity
{
    public Guid OrderId { get; private set; }
    public string ProductName { get; private set; } = default!;
    public decimal Price { get; private set; }
    public int Quantity { get; private set; }

    private OrderItem() { } // EF

    public OrderItem(Guid orderId, string productName, decimal price, int quantity)
    {
        OrderId = orderId;
        ProductName = productName;
        Price = price;
        Quantity = quantity;
    }
}