using EventDriven.OrderProcessing.Domain.Orders;

namespace EventDriven.OrderProcessing.Application.Common.Interfaces;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken cancellationToken);
    Task<Order?> GetIdByAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Order>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken
    );
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
