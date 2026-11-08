using Mango.Services.ProductAPI.Application__BLL_.Models;

namespace Mango.Services.ProductAPI.Application__BLL_.Contracts
{
  public interface IProductService
  {
    Task<IEnumerable<Product>> GetProductsAsync();
    Task<Product> GetProductByIdAsync(Guid productId);
    Task<Guid> CreateProductAsync(Product product);
    Task<Product> UpdateProductAsync(Product product);
    Task<bool> DeleteProductByIdAsync(Guid productId);
  }
}
