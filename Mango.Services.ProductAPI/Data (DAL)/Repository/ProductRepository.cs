using Mango.Services.ProductAPI.Data.DbContexts;
using Mango.Services.ProductAPI.Data.Entities;
using Mango.Services.ProductAPI.Shared.DTO;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.Data__DAL_.Repository
{
  public class ProductRepository : IProductRepository
  {
    private readonly ApplicationDbContext _context;
    public ProductRepository(ApplicationDbContext context)
    {
      _context = context;
    }

    /// <summary>
    /// Получить Продукт по идентификатору
    /// </summary>
    /// <param name="productId">Guid продукта</param>
    /// <returns>Ссылка на продукт либо null</returns>
    public async Task<ProductOutputDto?> GetProductByIdAsync(Guid productId) => 
      await _context.Products
      .Where(p => p.ProductId == productId)
      .Select(p => ProductMapper.ToProductOutputDto(p)).
      FirstOrDefaultAsync();

    /// <summary>
    /// Получить список всех продуктов
    /// </summary>
    /// <returns>Списко продуктов</returns>
    public async Task<IReadOnlyList<ProductOutputDto?>> GetProductsAsync() =>
      _context.Products.Select(p => ProductMapper.ToProductOutputDto(p)).ToList() ?? [];

    /// <summary>
    /// Удалить Продукт из БД
    /// </summary>
    /// <param name="productId">Идентификатор Продукта</param>
    /// <returns>истина или ложь</returns>
    public async Task<bool> DeleteProductAsync(Guid productId)
    {
      var productInDb = await GetProductAsync(productId);
      _context.Products.Remove(productInDb!);
      await _context.SaveChangesAsync();
      return true;
    }

    /// <summary>
    /// Добавить новый Продукт в БД
    /// </summary>
    /// <param name="productForCreation">DTO модель для создания продукта</param>
    /// <returns>Добавленный Продукт в БД</returns>
    public async Task<ProductOutputDto?> CreateProductAsync(ProductForCreationDto productForCreation)
    {
      var newProduct = ProductMapper.ToEntityProduct(productForCreation);
      newProduct?.ProductId = Guid.NewGuid();
      var result = _context.Products.Add(newProduct!);
      await _context.SaveChangesAsync();
      return ProductMapper.ToProductOutputDto(result.Entity);
    }

    /// <summary>
    /// Обновить Продукт в БД
    /// </summary>
    /// <param name="productId">Идентификатор Продукта</param>
    /// <param name="product">DTO модель для создания Продукта</param>
    /// <returns></returns>
    public async Task<ProductOutputDto?> UpdateProductAsync(Guid productId, ProductForUpdateDto product)
    {
      var productInDb = await GetProductAsync(productId);
      ProductMapper.UpdateEntity(product, productInDb);
      await _context.SaveChangesAsync();
      return ProductMapper.ToProductOutputDto(productInDb);
    }

    /// <summary>
    /// Получить сущность Продукт
    /// </summary>
    /// <param name="productId">Идентификатор Продукта</param>
    /// <returns>Ссылка на сущность</returns>
    public async Task<Product?> GetProductAsync(Guid productId) =>
      await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
  }
}
