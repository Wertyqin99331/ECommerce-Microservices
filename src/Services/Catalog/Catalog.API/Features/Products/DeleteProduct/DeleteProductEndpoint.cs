using Microsoft.AspNetCore.Mvc;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Catalog.API.Features.Products.DeleteProduct;

internal record DeleteProductRequest(Guid Id);

internal class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{id:guid}", DeleteProductAsync)
            .WithName("Delete product")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .WithSummary("Delete a product")
            .WithDescription("Delete a product")
            .WithTags("Products");
    }

    private static async Task<IResult> DeleteProductAsync([FromRoute] Guid id, ISender sender)
    {
        var command = new DeleteProductCommand(id);
        
        var result = await sender.Send(command);
        return result.IsSuccess ? Results.NoContent() : Results.BadRequest(result.Error.ToProblemDetails());
    }
}