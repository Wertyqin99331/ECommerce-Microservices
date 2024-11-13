using Catalog.API.Models;
using Marten.Pagination;
using GetProductsResult =
    CSharpFunctionalExtensions.Result<System.Collections.Generic.IEnumerable<Catalog.API.Models.Product>,
        Core.Errors.ApplicationError>;

namespace Catalog.API.Features.Products.GetProducts;

internal record GetProductsQuery(int Page = 1, int PageSize = 10) : IQuery<GetProductsResult>;

internal class GetProductsQueryHandler(IDocumentStore store) : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        await using var session = store.LightweightSession();

        var products = await session.Query<Product>()
            .ToPagedListAsync(request.Page, request.PageSize, cancellationToken);
        return Result.Success<IEnumerable<Product>, ApplicationError>(products);
    }
}