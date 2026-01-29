using EventDriven.OrderProcessing.Application.Orders.Queries.Services;
using MediatR;

namespace EventDriven.OrderProcessing.Application.Orders.Queries.GetOrderById;
public sealed class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDetailsDto?>
{
    private readonly IOrderQueryService _queryService;

    public GetOrderByIdQueryHandler(IOrderQueryService queryService)
    {
        _queryService = queryService;
    }

    public Task<OrderDetailsDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        return _queryService.GetByIdAsync(request.OrderId, cancellationToken);
    }
}
