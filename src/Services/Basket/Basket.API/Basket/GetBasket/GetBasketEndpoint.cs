namespace Basket.API.Basket.GetBasket;

public class GetBasketEndpoint : ICarterModule
{
    public record GetBasketResponse(ShoppingCart Cart);
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{username}", async (string username, ISender sender) => {
         
            var result= await sender.Send(new GetBasketQuery(username));

            var response =  result.Adapt<GetBasketResponse>();

            return Results.Ok(response);

        })      .WithName("GetBasketByUserName")
                .Produces<GetBasketResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get Basket By UserName")
                .WithDescription("Get Basket By UserName"); ;
    }
}
