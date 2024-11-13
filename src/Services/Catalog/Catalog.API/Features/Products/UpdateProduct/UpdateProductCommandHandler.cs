using Catalog.API.Models;

namespace Catalog.API.Features.Products.UpdateProduct;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    List<string> Categories,
    string Description,
    string ImageFile,
    decimal Price) : ICommand<UnitResult<ApplicationError>>;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(50).WithMessage("Name cannot be longer than 50 characters");

        RuleFor(x => x.Categories)
            .NotEmpty().WithMessage("Categories are required")
            .Must(c => c.Count > 0).WithMessage("Categories cannot be empty");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required");
        
        RuleFor(x => x.ImageFile)
            .NotEmpty().WithMessage("ImageFile is required");
        
        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

internal class UpdateProductCommandHandler(IDocumentStore store)
    : ICommandHandler<UpdateProductCommand, UnitResult<ApplicationError>>
{
    public async Task<UnitResult<ApplicationError>> Handle(UpdateProductCommand request,
        CancellationToken cancellationToken)
    {
        await using var session = store.LightweightSession();
        var product = await session.LoadAsync<Product>(request.Id, cancellationToken);

        if (product is null)
        {
            return ApplicationError.BadRequest("Product not found");
        }

        request.Adapt(product);

        session.Update(product);
        await session.SaveChangesAsync(cancellationToken);

        return UnitResult.Success<ApplicationError>();
    }
}