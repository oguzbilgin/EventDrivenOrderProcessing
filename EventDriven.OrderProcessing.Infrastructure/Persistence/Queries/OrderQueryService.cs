using EventDriven.OrderProcessing.Application.Orders.Queries.GetOrderById;
using EventDriven.OrderProcessing.Application.Orders.Queries.GetOrders;
using EventDriven.OrderProcessing.Application.Orders.Queries.Services;
using Microsoft.EntityFrameworkCore;

namespace EventDriven.OrderProcessing.Infrastructure.Persistence.Queries;
public sealed class OrderQueryService : IOrderQueryService
{
    private readonly AppDbContext _context;

    public OrderQueryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<OrderDetailsDto?> GetByIdAsync(Guid orderId, CancellationToken cancellation)
    {
        return await _context.Orders
            .AsNoTracking()
            .Where(order => order.Id == orderId)
            .Select(order => new OrderDetailsDto
            {
                Id = order.Id,
                TotalAmount = order.TotalAmount,
                Status = order.Status.ToString(),
                Items = order.Items.Select(item => new OrderItemDto
                {
                    ProductName = item.ProductName,
                    Quantity = item.Quantity
                }).ToList()
            })
            .FirstOrDefaultAsync(cancellation);
    }

    public async Task<IReadOnlyList<OrderListItemDto>> GetPagedAsync(int page, int pageSize, CancellationToken cancellation)
    {
        return await _context.Orders
            .AsNoTracking()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(order => new OrderListItemDto
            {
                Id = order.Id,
                TotalAmount = order.TotalAmount
            })
            .ToListAsync(cancellation);
    }
}
