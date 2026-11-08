using Mango.Services.ProductAPI.Application__BLL_.Contracts;
using Mango.Services.ProductAPI.Application__BLL_.Models;

namespace Mango.Services.ProductAPI.Application__BLL_
{
  public class ProductService : IProductService
  {
    public Task<Product> CreateUpdateProduct(Product product)
    {
      throw new NotImplementedException();
    }

    public Task<bool> DeleteProductById(Guid productId)
    {
      throw new NotImplementedException();
    }

    public Task<Product> GetProductById(Guid productId)
    {
      throw new NotImplementedException();
    }

    public Task<IEnumerable<Product>> GetProducts()
    {
      throw new NotImplementedException();
    }
  }
}
