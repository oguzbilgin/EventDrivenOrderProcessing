using MediatR;

namespace EventDriven.OrderProcessing.Application.Payments.Commands;

public record ProcessPaymentCommand(
    Guid OrderId,
    decimal Amount) : IRequest;
