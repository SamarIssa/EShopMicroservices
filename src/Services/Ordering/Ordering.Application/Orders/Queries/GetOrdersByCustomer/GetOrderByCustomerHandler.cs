
using Ordering.Application.Orders.Queries.GetOrdersByName;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Ordering.Application.Orders.Queries.GetOrdersByCustomer;

internal class GetOrderByCustomerHandler(IApplicationDbContext applicationDbContext) :
    IQueryHandler<GetOrderByCustomerQuery, GetOrderByCustomerResult>
{
    public async Task<GetOrderByCustomerResult> Handle(GetOrderByCustomerQuery query, CancellationToken cancellationToken)
    {
        var orderList = await applicationDbContext.Orders.Include(o => o.OrderItems)
           .AsNoTracking()
           .Where(x => x.CustomerId== CustomerId.Of(query.CustomerId))
           .OrderBy(x => x.OrderName.Value)
           .ToListAsync(cancellationToken);

        return new GetOrderByCustomerResult(orderList.ToOrderDtoList());
    }
}
