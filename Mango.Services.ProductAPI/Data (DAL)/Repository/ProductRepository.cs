using Microsoft.EntityFrameworkCore;

using Mango.Services.ProductAPI.Data.DbContexts;
using Mango.Services.ProductAPI.Data.Entities;

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
    public async Task<Product?> GetProductByIdAsync(Guid productId) => 
      await _context.Products.Where(p => p.ProductId == productId).FirstOrDefaultAsync();

    /// <summary>
    /// Получить список всех продуктов
    /// </summary>
    /// <returns>Списко продуктов</returns>
    public async Task<IReadOnlyList<Product>> GetProductsAsync() =>
      await _context.Products.AsNoTracking().ToListAsync();

  }
}
