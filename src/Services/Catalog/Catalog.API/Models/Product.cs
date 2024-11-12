using System.Text.Json.Serialization;

namespace Catalog.API.Models;

public class Product
{
    [JsonInclude]
    public Guid Id { get; private set; }
    public string Name { get; set; } = default!;
    public List<string> Categories { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string ImageFile { get; set; } = default!;
    public decimal Price { get; set; }
}