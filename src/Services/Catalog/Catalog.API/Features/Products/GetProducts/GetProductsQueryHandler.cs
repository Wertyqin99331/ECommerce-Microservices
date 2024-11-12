using Catalog.API.Models;

using GetProductsResult =
    CSharpFunctionalExtensions.Result<System.Collections.Generic.IReadOnlyList<Catalog.API.Models.Product>,
        Core.Errors.ApplicationError>;

namespace Catalog.API.Features.Products.GetProducts;

internal record GetProductsQuery(int Page, int CountPerPage) : IQuery<GetProductsResult>;

internal class GetProductsQueryHandler(IDocumentStore store) : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        await using var session = store.LightweightSession();

        var products = await session.Query<Product>()
            .Skip((request.Page - 1) * request.CountPerPage)
            .Take(request.CountPerPage)
            .ToListAsync(cancellationToken);

        return Result.Success<IReadOnlyList<Product>, ApplicationError>(products);
    }
}