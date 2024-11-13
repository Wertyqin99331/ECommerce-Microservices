using Catalog.API.Models;

namespace Catalog.API.Features.Products.CreateProduct;

using CreateProductResult = Result<Guid, ApplicationError>;

public record CreateProductCommand(
    string Name,
    List<string> Categories,
    string Description,
    string ImageFile,
    decimal Price) : ICommand<CreateProductResult>;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(2, 50).WithMessage("Name should be between 2 and 50 characters");

        RuleFor(x => x.Categories)
            .NotEmpty().WithMessage("Categories are required")
            .Must(c => c.Count > 0).WithMessage("Categories cannot be empty");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(500).WithMessage("Description cannot be longer than 500 characters");

        RuleFor(x => x.ImageFile)
            .NotEmpty().WithMessage("ImageFile is required");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

internal sealed class CreateProductCommandHandler(IDocumentStore store)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        await using var session = store.LightweightSession();

        var product = new Product
        {
            Name = request.Name,
            Categories = request.Categories,
            Description = request.Description,
            ImageFile = request.ImageFile,
            Price = request.Price
        };

        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}