

namespace Ordering.Application.Exceptions;

public class OrderNotFoundException: NotFoundException
{
    public OrderNotFoundException(Guid OrderId):base("Order", OrderId)
    {
        
    }
}
