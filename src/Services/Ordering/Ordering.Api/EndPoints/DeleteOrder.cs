
namespace Ordering.Api.EndPoints;


public record DeleteOrderResponse(Guid Id);
public class DeleteOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/orders/{id}",async(Guid id,))
    }
}
