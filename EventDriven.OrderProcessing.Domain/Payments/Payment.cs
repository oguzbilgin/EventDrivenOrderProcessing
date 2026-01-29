namespace EventDriven.OrderProcessing.Domain.Payments;

public sealed class Payment
{
    public Guid OrderId { get; private set; }
    public bool IsSuccessful { get; private set; }
    public DateTime ProcessedAt { get; private set; }

    private Payment() { } // EF

    public Payment(Guid orderId, bool isSuccessful)
    {
        OrderId = orderId;
        IsSuccessful = isSuccessful;
        ProcessedAt = DateTime.UtcNow;
    }
}
