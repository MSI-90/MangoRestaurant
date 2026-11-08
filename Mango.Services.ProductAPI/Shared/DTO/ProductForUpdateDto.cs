namespace Mango.Services.ProductAPI.Shared.DTO
{
  public record ProductForUpdateDto : ProductForCreationDto
  {
    public Guid ProductId { get; set; }
  }
}
