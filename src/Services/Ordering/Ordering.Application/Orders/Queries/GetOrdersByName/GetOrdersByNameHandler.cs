namespace Ordering.Application.Orders.Queries.GetOrdersByName;

internal class GetOrdersByNameHandler(IApplicationDbContext applicationDbContext) : IQueryHandler<GetOrdersByNameQuery, GetOrderByNameResult>
{
    public async Task<GetOrderByNameResult> Handle(GetOrdersByNameQuery query, CancellationToken cancellationToken)
    {
        var orderList = await applicationDbContext.Orders.Include(o => o.OrderItems)
             .AsNoTracking()
             .Where(x => x.OrderName.Value.Contains(query.Name))
             .OrderBy(x => x.OrderName.Value)
             .ToListAsync(cancellationToken);

        return new GetOrderByNameResult(orderList.ToOrderDtoList());
    }
}
