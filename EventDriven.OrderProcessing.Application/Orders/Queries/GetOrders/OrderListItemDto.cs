namespace EventDriven.OrderProcessing.Application.Orders.Queries.GetOrders;
public sealed class OrderListItemDto
{
    public Guid Id { get; init; }
    public decimal TotalAmount { get; init; }
    public string Status { get; init; } = default!;
}
