

namespace Ordering.Application.Orders.Commands.UpdateOrder;

internal class UpdateOrderHandler(IApplicationDbContext applicationDbContext) : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
{
    public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        var orderId = OrderId.Of(command.Order.Id);

        var order=await applicationDbContext.Orders.FindAsync([orderId],cancellationToken:cancellationToken);

        if(order is null)
        {
            throw new OrderNotFoundException(command.Order.Id);
        }

        UpdateOrderWithNewValues(order, command.Order);

        applicationDbContext.Orders.Update(order);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return new UpdateOrderResult(true);
    }

    private void UpdateOrderWithNewValues(Order order, OrderDto orderDto)
    {
        var updateShippingAddress = Address.Of(orderDto.ShippingAddress.FirstName, orderDto.ShippingAddress.LastName, orderDto.ShippingAddress.EmailAddress, orderDto.ShippingAddress.AddressLine,
          orderDto.ShippingAddress.Country, orderDto.ShippingAddress.State, orderDto.ShippingAddress.ZipCode);

        var updateBillingAddress = Address.Of(orderDto.BillingAddress.FirstName, orderDto.BillingAddress.LastName, orderDto.BillingAddress.EmailAddress, orderDto.BillingAddress.AddressLine,
           orderDto.BillingAddress.Country, orderDto.BillingAddress.State, orderDto.BillingAddress.ZipCode);

        var updatePayment = Payment.Of(orderDto.Payment.CardName, orderDto.Payment.CardNumber, orderDto.Payment.Expiration, orderDto.Payment.Cvv, orderDto.Payment.PaymentMethod);

        order.Update(OrderName.Of(orderDto.OrderName), updateShippingAddress, updateBillingAddress, updatePayment, orderDto.Status);


    }
}
