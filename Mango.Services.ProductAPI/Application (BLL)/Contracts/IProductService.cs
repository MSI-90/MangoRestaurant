using Mango.Services.ProductAPI.Application__BLL_.Models;

namespace Mango.Services.ProductAPI.Application__BLL_.Contracts
{
  public interface IProductService
  {
    Task<IEnumerable<Product>> GetProducts();
    Task<Product> GetProductById(Guid productId);
    Task<Product> CreateUpdateProduct(Product product);
    Task<bool> DeleteProductById(Guid productId);
  }
}
