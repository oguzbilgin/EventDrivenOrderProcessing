using FluentValidation;

namespace EventDriven.OrderProcessing.Application.Orders.Commands.CreateOrder;
public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("Order must contain at least one item.");

        RuleForEach(x => x.Items).ChildRules(items =>
        {
            items.RuleFor(i => i.ProductName)
                .NotEmpty();

            items.RuleFor(i => i.Quantity)
                .GreaterThan(0);
        });
    }
}
