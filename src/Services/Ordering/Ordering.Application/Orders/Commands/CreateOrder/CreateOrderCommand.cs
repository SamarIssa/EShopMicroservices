

namespace Ordering.Application.Orders.Commands.CreateOrder;

public record CreateOrderCommand(OrderDto Order):ICommand<CreateOrderResult>;
public record CreateOrderResult(Guid Id);

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("Order Name is required");
        RuleFor(x => x.Order.CustomerID).NotEmpty().WithMessage("Customer is required");
        RuleFor(x => x.Order.OrderItems).NotEmpty().WithMessage("Order Items shouldnt to be empty");
    }
}