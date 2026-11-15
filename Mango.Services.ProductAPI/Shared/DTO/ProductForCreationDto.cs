using System.ComponentModel.DataAnnotations;

namespace Mango.Services.ProductAPI.Shared.DTO
{
  public record ProductForCreationDto
  {
    [Required(ErrorMessage = "Не указано наименование товара.")]
    public string Name { get; set; } = string.Empty;

    [Range(1, 1000000, ErrorMessage = "Цена должна быть между 1 и 1000000.")]
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public string? CategoryName { get; set; }
    public string? ImageUrl { get; set; }
  }
}
