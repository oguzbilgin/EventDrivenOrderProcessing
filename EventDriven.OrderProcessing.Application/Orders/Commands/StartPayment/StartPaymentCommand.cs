using MediatR;

namespace EventDriven.OrderProcessing.Application.Orders.Commands.StartPayment;

public sealed record StartPaymentCommand(Guid OrderId) : IRequest;
