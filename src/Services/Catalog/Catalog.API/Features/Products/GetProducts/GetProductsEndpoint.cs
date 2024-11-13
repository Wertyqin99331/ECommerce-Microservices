using Catalog.API.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Catalog.API.Features.Products.GetProducts;

public record GetProductsRequest(int Page = 1, int PageSize = 10);

public record GetProductsResponse(IEnumerable<Product> Products);

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", GetProductsAsync)
            .WithName("Get products")
            .Produces<GetProductsResponse>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .WithSummary("Get products")
            .WithDescription("Get products")
            .WithTags("Products");
    }

    private async Task<IResult> GetProductsAsync([AsParameters] GetProductsRequest request, ISender sender, IMapper mapper)
    {
        var query = new GetProductsQuery(request.Page, request.PageSize);

        var result = await sender.Send(query);
        if (result.IsSuccess)
        {
            return Results.Ok(new GetProductsResponse(result.Value));
        }

        return Results.BadRequest(result.Error.ToProblemDetails());
    }
}