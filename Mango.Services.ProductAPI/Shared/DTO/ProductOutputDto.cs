namespace Mango.Services.ProductAPI.Shared.DTO
{
  public record ProductOutputDto
  {
    public Guid ProductId { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string? Description { get; init; }
    public string? CategoryName { get; init; }
    public string? ImageUrl { get; init; }
  }
}
