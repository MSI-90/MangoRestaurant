using Mango.Services.ProductAPI.Data.Entities;

namespace Mango.Services.ProductAPI.Data__DAL_.Repository
{
  public interface IProductRepository
  {
    Task<Product?> GetProductByIdAsync(Guid productId);
  }
}
