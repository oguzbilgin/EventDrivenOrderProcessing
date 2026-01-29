namespace EventDriven.OrderProcessing.Application.Orders.Queries.GetOrders;
using AutoMapper;
using EventDriven.OrderProcessing.Domain.Orders;

public sealed class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        CreateMap<Order, OrderListItemDto>()
            .ForMember(
                dest => dest.Status,
                opt => opt.MapFrom(src => src.Status.ToString())
            );
    }
}
