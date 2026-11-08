using System.ComponentModel.DataAnnotations;

namespace Mango.Services.ProductAPI.Data.Entities
{
  public class Product
  {
    public Guid ProductId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Range(typeof(decimal), "1", "1000000000")]
    public decimal Price { get; set; }
    public string? Description { get; set; }

    [MaxLength(150)]
    public string? CategoryName { get; set; }
    public string? ImageUrl { get; set; }
  }
}
