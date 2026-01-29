using EventDriven.OrderProcessing.Application.Common.Interfaces;
using EventDriven.OrderProcessing.Domain.Orders;
using EventDriven.OrderProcessing.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventDriven.OrderProcessing.Infrastructure.Repository;

public sealed class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;
    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(Order order, CancellationToken cancellationToken)
    {
        await _context.Orders.AddAsync(order, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Order>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _context.Orders
            .Where(o => o.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Order?> GetIdByAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
