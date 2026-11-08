using System.ComponentModel.DataAnnotations;

namespace Mango.Services.ProductAPI.Application__BLL_.Models
{
  public class Product
  {
    public Guid ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public string? CategoryName { get; set; }
    public string? ImageUrl { get; set; }
  }
}
