using Microsoft.AspNetCore.Mvc;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Catalog.API.Features.Products.UpdateProduct;

internal record UpdateProductRequest(
    string Name,
    List<string> Categories,
    string Description,
    string ImageFile,
    decimal Price);

internal class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products/{id:guid}", UpdateProductAsync)
            .WithName("Update product")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .WithSummary("Update a product")
            .WithDescription("Update a product")
            .WithTags("Products");
    }

    private static async Task<IResult> UpdateProductAsync(Guid id, [FromBody] UpdateProductRequest request,
        ISender sender)
    {
        var command = new UpdateProductCommand(id, request.Name, request.Categories, request.Description,
            request.ImageFile, request.Price);

        var result = await sender.Send(command);
        return result.IsSuccess ? Results.NoContent() : Results.BadRequest(result.Error.ToProblemDetails());
    }
}