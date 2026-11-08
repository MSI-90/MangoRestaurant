namespace Mango.Services.ProductAPI.Shared.DTO
{
  public record ProductOutputDto : ProductForCreationDto
  {
    public Guid ProductId { get; init; }
  }
}
