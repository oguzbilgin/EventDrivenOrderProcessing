using EventDriven.OrderProcessing.Application.Orders.Queries.GetOrderById;
using EventDriven.OrderProcessing.Application.Orders.Queries.GetOrders;

namespace EventDriven.OrderProcessing.Application.Orders.Queries.Services;
public interface IOrderQueryService
{
    Task<OrderDetailsDto?> GetByIdAsync(
        Guid orderId,
        CancellationToken cancellation
    );

    Task<IReadOnlyList<OrderListItemDto>> GetPagedAsync(
        int page,
        int pageSize,
        CancellationToken cancellation
    );
}
