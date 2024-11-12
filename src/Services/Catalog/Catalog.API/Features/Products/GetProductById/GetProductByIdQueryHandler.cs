using Catalog.API.Models;

namespace Catalog.API.Features.Products.GetProductById;

internal record GetProductByIdQuery(Guid Id) : IQuery<Result<Product, ApplicationError>>;

internal class GetProductByIdQueryHandler(IDocumentStore store): IQueryHandler<GetProductByIdQuery, Result<Product, ApplicationError>>
{
    public async Task<Result<Product, ApplicationError>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        await using var session = store.LightweightSession();

        var product = await session.LoadAsync<Product>(request.Id, cancellationToken);
        if (product is null)
        {
            return ApplicationError.BadRequest("Product not found");
        }

        return product;
    }
}