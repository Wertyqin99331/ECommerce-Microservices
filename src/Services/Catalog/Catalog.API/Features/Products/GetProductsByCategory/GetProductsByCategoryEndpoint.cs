using Catalog.API.Models;
using Microsoft.AspNetCore.Mvc;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Catalog.API.Features.Products.GetProductsByCategory;

internal class GetProductsByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{category}", GetProductsByCategoryAsync)
            .WithName("Get products by category")
            .Produces<IEnumerable<Product>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .WithSummary("Get products by category")
            .WithDescription("Get products by category")
            .WithTags("Products");
    }

    private static async Task<IResult> GetProductsByCategoryAsync([FromRoute] string category, ISender sender)
    {
        var query = new GetProductsByCategoryQuery(category);

        var result = await sender.Send(query);
        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        return Results.BadRequest(result.Error.ToProblemDetails());
    }
}