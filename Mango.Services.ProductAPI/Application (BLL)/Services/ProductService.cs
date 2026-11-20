using Mango.Services.ProductAPI.Application__BLL_.Contracts;
using Mango.Services.ProductAPI.Application__BLL_.Exceptions;
using Mango.Services.ProductAPI.Data.DbContexts;
using Mango.Services.ProductAPI.Data__DAL_.Repository;
using Mango.Services.ProductAPI.Shared.DTO;

namespace Mango.Services.ProductAPI.Application__BLL_
{
  public sealed class ProductService : IProductService
  {
    private readonly ApplicationDbContext _context;
    private readonly IProductRepository _productRep;
    public ProductService(ApplicationDbContext context, IProductRepository productRep)
    {
      _context = context;
      _productRep = productRep;
    }

    /// <summary>
    /// Добавить новый продукт в БД
    /// </summary>
    /// <param name="productForCreation">DTO модели для создания продукта</param>
    /// <returns>DTO модели продукта добавленного в БД</returns>
    public async Task<ProductOutputDto?> CreateProductAsync(ProductForCreationDto productForCreation) => 
      await _productRep.CreateProductAsync(productForCreation);
    
    /// <summary>
    /// Обновить информацию о продукте
    /// </summary>
    /// <param name="productId">Guid продукта</param>
    /// <param name="product">DTO модели для обновления продукта</param>
    /// <returns>DTO модели продукта обновленного в БД</returns>
    public async Task<ProductOutputDto?> UpdateProductAsync(Guid productId, ProductForUpdateDto product)
    {
      await GetProductAndCheckIfExists(productId);
      return await _productRep.UpdateProductAsync(productId, product);
    }

    /// <summary>
    /// Удалить Продукт из БД по идентификатору
    /// </summary>
    /// <param name="productId">Идентификатор Продукта</param>
    /// <returns></returns>
    public async Task<bool> DeleteProductByIdAsync(Guid productId)
    {
      await GetProductAndCheckIfExists(productId);
      return await _productRep.DeleteProductAsync(productId);
    }

    /// <summary>
    /// Получить продукт по идентификатору
    /// </summary>
    /// <param name="productId">Guid продукта</param>
    /// <returns>DTO модель ProductOutputDto</returns>
    public async Task<ProductOutputDto?> GetProductByIdAsync(Guid productId) => 
      await GetProductAndCheckIfExists(productId);

    /// <summary>
    /// Получить весь набор данных о продуктах из БД
    /// </summary>
    /// <returns></returns>
    public async Task<IReadOnlyList<ProductOutputDto?>> GetProductsAsync() => 
      await _productRep.GetProductsAsync();

    /// <summary>
    /// Проверка на наличие в БД Продукта по Guid эквивалентному переданному в качестве параметра и возврат его в виде результата
    /// </summary>
    /// <param name="id">Guid продукта</param>
    /// <returns>Продукт</returns>
    /// <exception cref="ProductNotFoundException">Кастомное исключение в виде ответа в случае отсутствия продукта</exception>
    private async Task<ProductOutputDto?> GetProductAndCheckIfExists(Guid id) => 
      await _productRep.GetProductByIdAsync(id) ?? throw new ProductNotFoundException(id);
  }
}
