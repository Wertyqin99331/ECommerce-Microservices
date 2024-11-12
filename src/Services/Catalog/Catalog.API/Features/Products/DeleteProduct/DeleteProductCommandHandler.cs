using Catalog.API.Models;

namespace Catalog.API.Features.Products.DeleteProduct;

internal record DeleteProductCommand(Guid Id) : ICommand<UnitResult<ApplicationError>>;

internal class DeleteProductCommandHandler(IDocumentStore store): ICommandHandler<DeleteProductCommand, UnitResult<ApplicationError>>
{
    public async Task<UnitResult<ApplicationError>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        await using var session = store.LightweightSession();

        var product = await session.LoadAsync<Product>(request.Id, cancellationToken);
        if (product is null)
            return ApplicationError.BadRequest("Product not found");
        
        session.Delete(product);
        await session.SaveChangesAsync(cancellationToken);
        
        return UnitResult.Success<ApplicationError>();
    }
}