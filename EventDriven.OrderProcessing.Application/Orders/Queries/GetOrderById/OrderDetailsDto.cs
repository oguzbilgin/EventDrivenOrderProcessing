namespace EventDriven.OrderProcessing.Application.Orders.Queries.GetOrderById;
public sealed class OrderDetailsDto
{
    public Guid Id { get; init; }
    public decimal TotalAmount { get; init; }
    public string Status { get; init; } = default!;

    public IReadOnlyList<OrderItemDto> Items { get; init; } = [];
}

public sealed class OrderItemDto
{
    public string ProductName { get; init; } = default!;
    public int Quantity { get; init; }
}
