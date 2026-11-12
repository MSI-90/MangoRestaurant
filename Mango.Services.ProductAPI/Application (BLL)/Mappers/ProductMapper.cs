using Mango.Services.ProductAPI.Data.Entities;
using Mango.Services.ProductAPI.Shared.DTO;

namespace Mango.Services.ProductAPI.Application__BLL_.Mappers
{
  public static class ProductMapper
  {
    internal static Product? ToEntityProduct(this ProductForCreationDto? productForCreation) =>
      productForCreation is not null ? new Product
      {
        ProductId = Guid.NewGuid(),
        Name = productForCreation.Name,
        Price = productForCreation.Price,
        Description = productForCreation.Description,
        CategoryName = productForCreation.CategoryName,
        ImageUrl = productForCreation.ImageUrl
      } : null;

    internal static ProductOutputDto? ToProductOutputDto(this Product? product) =>
      product is not null ? new ProductOutputDto
      {
        ProductId = product.ProductId,
        Name = product.Name,
        Price = product.Price,
        Description = product.Description,
        CategoryName = product.CategoryName,
        ImageUrl = product.ImageUrl
      } : null;
  }
}
