

using System.Net.Sockets;

namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(string Name, string Description,string ImageFile, decimal Price,List<string> Category):ICommand<CreateProductResult>;
public record CreateProductResult(Guid Id);
internal class CreateProductHandler(IDocumentSession session) : IRequestHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {

        var product = new Product()
        {
            Name = command.Name,
            ImageFile = command.ImageFile,
            Price = command.Price,
            Description = command.Description,
            Category = command.Category
        };
        //Save database
        session.Store(product);

        await session.SaveChangesAsync(cancellationToken);

        return new CreateProductResult(product.Id);
    }
    //docker ps-----> "to get container ids"
    // docker exec -it 6e87eb07dcb7 bash
    //psql -U postgres
    // "\c CatalogDb" connect db
    // "\d" check table
    //SELECT * FROM mt_doc_product;
}
