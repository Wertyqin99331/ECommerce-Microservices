using Catalog.API.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Catalog.API.Features.Products.GetProducts;

internal record GetProductsResponse(IEnumerable<Product> Products);

internal class GetProductsEndpoint: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", GetProductsAsync)
            .WithName("Get products")
            .Produces<GetProductsResponse>()
            .Produces<ApplicationError>(StatusCodes.Status400BadRequest)
            .WithSummary("Get products")
            .WithDescription("Get products")
            .WithTags("Products");
    }

    private async Task<IResult> GetProductsAsync([FromQuery] int page, [FromQuery] int countPerPage,
        ISender sender, IMapper mapper)
    {
        var query = new GetProductsQuery(page, countPerPage);

        var result = await sender.Send(query);
        if (result.IsSuccess)
        {
            return Results.Ok(new GetProductsResponse(result.Value));
        }

        return Results.BadRequest(result.Error);
    }
}