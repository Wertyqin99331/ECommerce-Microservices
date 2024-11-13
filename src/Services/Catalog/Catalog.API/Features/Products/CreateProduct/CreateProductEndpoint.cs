using Microsoft.AspNetCore.Mvc;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Catalog.API.Features.Products.CreateProduct;

internal record CreateProductRequest(
    string Name,
    List<string> Categories,
    string Description,
    string ImageFile,
    decimal Price);

internal record CreateProductResponse(Guid Id);

internal class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", CreateProductAsync)
            .WithName("Create product")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .WithSummary("Create a product")
            .WithDescription("Create a product")
            .WithTags("Products");
    }

    private static async Task<IResult> CreateProductAsync([FromBody] CreateProductRequest request, ISender sender)
    {
        var command = request.Adapt<CreateProductCommand>();

        var result = await sender.Send(command);
        if (result.IsSuccess)
        {
            var response = new CreateProductResponse(result.Value);
            return Results.Created($"/products/{result.Value}", response);
        }

        return Results.BadRequest(result.Error.ToProblemDetails());
    }
}