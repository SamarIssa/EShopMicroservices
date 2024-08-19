
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Ordering.Application.Orders.Commands.DeleteOrder;

public class DeleteOrderHandler(IApplicationDbContext applicationDbContext) : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
{
    public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
    {
        var orderId = OrderId.Of(command.Id);

        var order = await applicationDbContext.Orders.FindAsync([orderId], cancellationToken: cancellationToken);

        if (order is null)
        {
            throw new OrderNotFoundException(command.Id);
        }

        applicationDbContext.Orders.Remove(order);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return new DeleteOrderResult(true);
    }
}
