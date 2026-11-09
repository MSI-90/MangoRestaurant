using Mango.Services.ProductAPI.Application__BLL_.Models;
using Mango.Services.ProductAPI.Shared.DTO;

namespace Mango.Services.ProductAPI.Application__BLL_.Contracts
{
  public interface IProductService
  {
    Task<IEnumerable<ProductOutputDto>> GetProductsAsync();
    Task<ProductOutputDto> GetProductByIdAsync(Guid productId);
    Task<Guid> CreateProductAsync(ProductForCreationDto product);
    Task<ProductOutputDto> UpdateProductAsync(Guid productId, ProductForUpdateDto product);
    Task<bool> DeleteProductByIdAsync(Guid productId);
  }
}
