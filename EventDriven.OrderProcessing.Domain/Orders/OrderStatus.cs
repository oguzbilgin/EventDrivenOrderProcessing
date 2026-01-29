namespace EventDriven.OrderProcessing.Domain.Orders;

public enum OrderStatus
{
    Pending = 0,
    PaymentPending = 1,
    Paid = 2,
    Completed = 3,
    Cancelled = 4,
    PaymentFailed = 5
}