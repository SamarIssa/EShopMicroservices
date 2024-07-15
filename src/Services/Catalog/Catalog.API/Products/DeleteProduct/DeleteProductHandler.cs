﻿using Catalog.API.Products.UpdateProduct;

namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id):ICommand<DeleteProductResult>;
public record DeleteProductResult(bool IsSuccess);

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty().WithMessage("Product ID is required");

    }
}
public class DeleteProductCommandHandler(ILogger<DeleteProductCommandHandler> logger,IDocumentSession session) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("DeleteProductCommandHandler.Handle called with {@command}", command);

        var product = await session.LoadAsync<Product>(command.Id, cancellationToken);

        if (product == null)
        {
            throw new ProductNotFoundException(command.Id);
        }

        session.Delete(product);

        await session.SaveChangesAsync(cancellationToken);

        return new DeleteProductResult(true);
    }
}
