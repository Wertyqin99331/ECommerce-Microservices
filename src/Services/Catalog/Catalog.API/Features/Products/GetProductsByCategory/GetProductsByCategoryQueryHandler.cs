using Catalog.API.Models;

namespace Catalog.API.Features.Products.GetProductsByCategory;

internal record GetProductsByCategoryQuery(string Category) : IQuery<Result<IReadOnlyList<Product>, ApplicationError>>;

internal class GetProductsByCategoryQueryHandler(IDocumentStore store)
    : IQueryHandler<GetProductsByCategoryQuery, Result<IReadOnlyList<Product>, ApplicationError>>
{
    public async Task<Result<IReadOnlyList<Product>, ApplicationError>> Handle(GetProductsByCategoryQuery request,
        CancellationToken cancellationToken)
    {
        await using var session = store.LightweightSession();

        var products = await session.Query<Product>()
            .Where(p => p.Categories.Contains(request.Category))
            .ToListAsync(cancellationToken);

        return Result.Success<IReadOnlyList<Product>, ApplicationError>(products);
    }
}