﻿
using Ordering.Application.Orders.Commands.UpdateOrder;

namespace Ordering.Api.EndPoints;

public record UpdateOrderRequest(OrderDto OrderDto);
public record UpdateOrderResponse(bool IsSuccess);
public class UpdateOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/orders", async (UpdateOrderRequest request,ISender sender) => {

            var command = request.Adapt<UpdateOrderCommand>();

            var result=sender.Send(command);

            var response = result.Adapt<UpdateOrderResponse>();

            return Results.Ok(response);
        })
        .WithName("UpdateOrder")
        .Produces<UpdateOrderResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Update Order")
        .WithDescription("Update Order")
        ;
    }
}