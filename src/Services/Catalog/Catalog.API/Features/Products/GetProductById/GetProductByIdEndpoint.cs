using Catalog.API.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Catalog.API.Features.Products.GetProductById;

internal class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id:guid}", GetProductByIdAsync)
            .WithName("Get product by id")
            .Produces<Product>()
            .Produces<ApplicationError>(StatusCodes.Status400BadRequest)
            .WithSummary("Get a product by id")
            .WithDescription("Get a product by id")
            .WithTags("Products");
    }

    private static async Task<IResult> GetProductByIdAsync([FromRoute] Guid id, ISender sender, IMapper mapper)
    {
        var query = new GetProductByIdQuery(id);

        var result = await sender.Send(query);
        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        return Results.BadRequest(result.Error);
    }
}