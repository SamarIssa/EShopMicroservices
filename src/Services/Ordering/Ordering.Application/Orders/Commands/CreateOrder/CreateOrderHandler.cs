﻿

namespace Ordering.Application.Orders.Commands.CreateOrder;

internal class CreateOrderHandler(IApplicationDbContext applicationDbContext) : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = CreateNewOrder(command.Order);

        applicationDbContext.Orders.Add(order);

        await applicationDbContext.SaveChangesAsync(cancellationToken); 

        return new CreateOrderResult(order.Id.Value);
    }

    private Order CreateNewOrder(OrderDto order)
    {
       var shippingAddress=Address.Of(order.ShippingAddress.FirstName, order.ShippingAddress.LastName,order.ShippingAddress.EmailAddress,order.ShippingAddress.AddressLine,
           order.ShippingAddress.Country, order.ShippingAddress.State, order.ShippingAddress.ZipCode);

        var billingAddress= Address.Of(order.BillingAddress.FirstName, order.BillingAddress.LastName, order.BillingAddress.EmailAddress, order.BillingAddress.AddressLine,
           order.BillingAddress.Country, order.BillingAddress.State , order.BillingAddress.ZipCode);

        var newOrder = Order.Create(
            OrderId.Of(Guid.NewGuid()),
            CustomerId.Of(order.CustomerId),
            OrderName.Of(order.OrderName),
            shippingAddress, billingAddress,
            Payment.Of(order.Payment.CardName, order.Payment.CardNumber, order.Payment.Expiration,order.Payment.Cvv, order.Payment.PaymentMethod)
            );

        foreach( var orderItem in order.OrderItems)
        {
            newOrder.Add(ProductId.Of(orderItem.ProductId),orderItem.Quantity,orderItem.Price);
        }
        return newOrder;
    }
}
