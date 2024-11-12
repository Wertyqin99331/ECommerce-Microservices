using Catalog.API.Models;

namespace Catalog.API.Features.Products.UpdateProduct;

internal record UpdateProductCommand(
    Guid Id,
    string Name,
    List<string> Categories,
    string Description,
    string ImageFile,
    decimal Price) : ICommand<UnitResult<ApplicationError>>;

internal class UpdateProductCommandHandler(IDocumentStore store)
    : ICommandHandler<UpdateProductCommand, UnitResult<ApplicationError>>
{
    public async Task<UnitResult<ApplicationError>> Handle(UpdateProductCommand request,
        CancellationToken cancellationToken)
    {
        await using var session = store.LightweightSession();
        var product = await session.LoadAsync<Product>(request.Id, cancellationToken);

        if (product is null)
        {
            return ApplicationError.BadRequest("Product not found");
        }

        request.Adapt(product);

        session.Update(product);
        await session.SaveChangesAsync(cancellationToken);

        return UnitResult.Success<ApplicationError>();
    }
}