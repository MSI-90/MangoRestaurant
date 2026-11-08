namespace Mango.Services.ProductAPI.Shared.DTO
{
  public record ProductOutputDto : ProductDto
  {
    public Guid ProductId { get; init; }
  }
}
