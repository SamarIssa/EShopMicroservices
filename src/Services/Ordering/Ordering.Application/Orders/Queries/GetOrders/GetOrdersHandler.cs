
using BuildingBlocks.Pagination;

namespace Ordering.Application.Orders.Queries.GetOrders;

public class GetOrdersHandler(IApplicationDbContext applicationDbContext) : IQueryHandler<GetOrdersQuery, GetOrdersQueryResult>
{
    public async Task<GetOrdersQueryResult> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
       
        var pageIndex= request.PaginationRequest.PageIndex;
        var pageSize= request.PaginationRequest.PageSize;

        var count = await applicationDbContext.Orders.LongCountAsync(cancellationToken);

        var orders = await applicationDbContext.Orders.Include(x => x.OrderItems)
            .OrderBy(x => x.OrderName.Value)
            .Skip(pageIndex * pageSize)
            .Take(pageSize).ToListAsync(cancellationToken);

        return new GetOrdersQueryResult(new PaginatedResult<OrderDto>(pageIndex, pageSize, count, orders.ToOrderDtoList()));
        
    }
}
