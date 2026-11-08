namespace Mango.Services.ProductAPI.Shared.DTO
{
  public record ProductForUpdateDto : ProductDto
  {
    public Guid ProductId { get; set; }
  }
}
