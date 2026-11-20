using Mango.Services.ProductAPI.Data.Entities;
using Mango.Services.ProductAPI.Shared.DTO;

namespace Mango.Services.ProductAPI.Data__DAL_.Repository
{
  public interface IProductRepository
  {
    Task<ProductOutputDto?> GetProductByIdAsync(Guid productId);
    Task<IReadOnlyList<ProductOutputDto?>> GetProductsAsync();
    Task<Product?> GetProductAsync(Guid productId);
    Task<bool> DeleteProductAsync(Guid productId);
    Task<ProductOutputDto?> UpdateProductAsync(Guid productId, ProductForUpdateDto product);
    Task<ProductOutputDto?> CreateProductAsync(ProductForCreationDto productForCreation);
  }
}
