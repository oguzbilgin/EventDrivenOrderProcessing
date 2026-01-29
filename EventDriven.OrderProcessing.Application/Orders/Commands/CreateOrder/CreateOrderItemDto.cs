namespace EventDriven.OrderProcessing.Application.Orders.Commands.CreateOrder;

public record CreateOrderItemDto(
    string ProductName,
    decimal Price,
    int Quantity
);
