using Mango.Services.ProductAPI.Application__BLL_.Contracts;
using Mango.Services.ProductAPI.Application__BLL_.Exceptions;
using Mango.Services.ProductAPI.Application__BLL_.Mappers;
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
    public async Task<ProductOutputDto?> CreateProductAsync(ProductForCreationDto productForCreation)
    {
      var newProduct = ProductMapper.ToEntityProduct(productForCreation);
      newProduct?.ProductId = Guid.NewGuid();
      var result = _context.Products.Add(newProduct!);
      await _context.SaveChangesAsync();
      return ProductMapper.ToProductOutputDto(result.Entity);
    }
    
    /// <summary>
    /// Обновить информацию о продукте
    /// </summary>
    /// <param name="productId">Guid продукта</param>
    /// <param name="product">DTO модели для обновления продукта</param>
    /// <returns>DTO модели продукта обновленного в БД</returns>
    public async Task<ProductOutputDto?> UpdateProductAsync(Guid productId, ProductForUpdateDto product)
    {
      var productInDb = await GetProductAndCheckIfExists(productId);
      ProductMapper.UpdateEntity(product, productInDb);
      await _context.SaveChangesAsync();
      return await GetProductByIdAsync(productId);
    }

    /// <summary>
    /// Удалить Продукт из БД по идентификатору
    /// </summary>
    /// <param name="productId">Идентификатор Продукта</param>
    /// <returns></returns>
    public async Task<bool> DeleteProductByIdAsync(Guid productId)
    {
      var productInDb = await GetProductAndCheckIfExists(productId);
      _context.Products.Remove(productInDb!);
      await _context.SaveChangesAsync();
      return true;
    }

    /// <summary>
    /// Получить продукт по идентификатору
    /// </summary>
    /// <param name="productId">Guid продукта</param>
    /// <returns>DTO модель ProductOutputDto</returns>
    public async Task<ProductOutputDto?> GetProductByIdAsync(Guid productId)
    {
      var productInDb = await GetProductAndCheckIfExists(productId);
      return ProductMapper.ToProductOutputDto(productInDb);
    }

    /// <summary>
    /// Получить весь набор данных о продуктах из БД
    /// </summary>
    /// <returns></returns>
    public async Task<IReadOnlyList<ProductOutputDto?>> GetProductsAsync()
    {
      var products = await _productRep.GetProductsAsync();
      return products.Select(p => ProductMapper.ToProductOutputDto(p)).ToList() ?? [];
    }

    /// <summary>
    /// Проверка на наличие в БД Продукта по Guid эквивалентному переданному в качестве параметра и возврат его в виде результата
    /// </summary>
    /// <param name="id">Guid продукта</param>
    /// <returns>Продукт</returns>
    /// <exception cref="ProductNotFoundException">Кастомное исключение в виде ответа в случае отсутствия продукта</exception>
    private async Task<Data.Entities.Product?> GetProductAndCheckIfExists(Guid id)
    {
      var product = await _productRep.GetProductByIdAsync(id);
      return product ?? throw new ProductNotFoundException(id);
    }
  }
}
