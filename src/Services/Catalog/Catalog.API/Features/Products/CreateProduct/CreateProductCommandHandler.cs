using Catalog.API.Models;

namespace Catalog.API.Features.Products.CreateProduct;

using CreateProductResult = Result<Guid, ApplicationError>;

internal record CreateProductCommand(
    string Name,
    List<string> Categories,
    string Description,
    string ImageFile,
    decimal Price) : ICommand<CreateProductResult>;

internal sealed class CreateProductCommandHandler(IDocumentStore store)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        await using var session = store.LightweightSession();
        
        var product = new Product
        {
            Name = request.Name,
            Categories = request.Categories,
            Description = request.Description,
            ImageFile = request.ImageFile,
            Price = request.Price
        };

        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}