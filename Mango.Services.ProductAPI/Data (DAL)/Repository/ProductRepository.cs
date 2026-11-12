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

    public async Task<Product?> GetProductByIdAsync(Guid productId) => 
      await _context.Products.Where(p => p.ProductId == productId).FirstOrDefaultAsync();

  }
}
