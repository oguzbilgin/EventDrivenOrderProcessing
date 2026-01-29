using FluentValidation;

namespace EventDriven.OrderProcessing.Application.Orders.Commands.StartPayment;
public sealed class StartPaymentCommandValidator : AbstractValidator<StartPaymentCommand>
{
    public StartPaymentCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("OrderId is required.");
    }
}
