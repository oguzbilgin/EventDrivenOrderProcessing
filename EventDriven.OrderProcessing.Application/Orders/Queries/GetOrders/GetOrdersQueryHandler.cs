using AutoMapper;
using EventDriven.OrderProcessing.Application.Common.Interfaces;
using MediatR;

namespace EventDriven.OrderProcessing.Application.Orders.Queries.GetOrders;
public sealed class GetOrdersQueryHandler
    : IRequestHandler<GetOrdersQuery, IReadOnlyList<OrderListItemDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IMapper _mapper;

    public GetOrdersQueryHandler(
        IOrderRepository orderRepository,
        ICurrentUser currentUser,
        IMapper mapper
    )
    {
        _orderRepository = orderRepository;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<OrderListItemDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetByUserIdAsync(
            _currentUser.UserId,
            cancellationToken);

        return _mapper.Map<IReadOnlyList<OrderListItemDto>>(orders);
    }
}
