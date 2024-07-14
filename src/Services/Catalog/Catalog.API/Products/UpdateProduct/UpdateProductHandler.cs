namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(Guid Id,string Name, string Description, string ImageFile, decimal Price, List<string> Category) :ICommand<UpdateProductResult>;
public record UpdateProductResult(bool IsSuccess);
internal class UpdateProductCommandHandler(IDocumentSession session, ILogger<UpdateProductCommandHandler> logger) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("UpdateProductCommandHandler.Handle called with {@command}", command);

        var product=await session.LoadAsync<Product>(command.Id, cancellationToken);

        if (product == null) {
            throw new ProductNotFoundException();
        }

        product.Name = command.Name;
        product.ImageFile = command.ImageFile;
        product.Price = command.Price;
        product.Description = command.Description;
        product.Category = command.Category;

        session.Update(product);

        await session.SaveChangesAsync(cancellationToken);

        return new UpdateProductResult(true);
    }
}
