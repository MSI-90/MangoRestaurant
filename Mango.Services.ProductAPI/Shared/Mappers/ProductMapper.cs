using Mango.Services.ProductAPI.Data.Entities;

namespace Mango.Services.ProductAPI.Shared.DTO
{
  public static class ProductMapper
  {
    internal static Product? ToEntityProduct(this ProductForCreationDto? productForCreation) =>
      productForCreation is not null ? new Product
      {
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

    internal static void UpdateEntity(this ProductForUpdateDto? productToUpdate, Product? productInDb)
    {
      if (productToUpdate is null || productInDb is null) 
        return;

      productInDb.Name = productToUpdate.Name;
      productInDb.Price = productToUpdate.Price;
      productInDb.Description = productToUpdate.Description;
      productInDb.CategoryName = productToUpdate.CategoryName;
      productInDb.ImageUrl = productToUpdate.ImageUrl;
    }
  }
}
