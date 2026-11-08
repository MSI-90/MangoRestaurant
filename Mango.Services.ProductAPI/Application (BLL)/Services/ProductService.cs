using Mango.Services.ProductAPI.Application__BLL_.Contracts;
using Mango.Services.ProductAPI.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

using EntityProduct = Mango.Services.ProductAPI.Data.Entities.Product;
using Mango.Services.ProductAPI.Shared.DTO;

namespace Mango.Services.ProductAPI.Application__BLL_
{
  public class ProductService : IProductService
  {
    private readonly ApplicationDbContext _context;
    public ProductService(ApplicationDbContext context)
    {
      _context = context;
    }
    public async Task<Guid> CreateProductAsync(ProductForCreationDto product)
    {
      var newProduct = new EntityProduct
      {
        ProductId = Guid.NewGuid(),
        Name = product.Name,
        Price = product.Price,
        Description = product.Description,
        CategoryName = product.CategoryName,
        ImageUrl = product.ImageUrl
      };
      
      var result = await _context.Products.AddAsync(newProduct);
      _context.SaveChanges();
      return result.Entity.ProductId;
    }

    public async Task<ProductOutputDto> UpdateProductAsync(ProductForUpdateDto product)
    {
      var productInDb = await GetProductByIdAsync(product.ProductId) ?? throw new InvalidOperationException("Product not found");
      var productToUpdate = new EntityProduct
      {
        CategoryName = product.CategoryName,
        Description = product.Description,
        Name = product.Name,
        ImageUrl = product.ImageUrl,
        Price = product.Price,
        ProductId = product.ProductId
      };

      _context.Products.Update(productToUpdate);
      await _context.SaveChangesAsync();

      return await GetProductByIdAsync(product.ProductId);
    }

    public async Task<bool> DeleteProductByIdAsync(Guid productId)
    {
      var product = await GetProductByIdAsync(productId);
      if (product is null) return false;

      _context.Products.Remove(new EntityProduct
      {
        ProductId = product.ProductId,
        Name = product.Name,
        Price = product.Price,
        Description = product.Description,
        CategoryName = product.CategoryName,
        ImageUrl = product.ImageUrl
      });

      await _context.SaveChangesAsync();
      return true;
    } 

    public async Task<ProductOutputDto> GetProductByIdAsync(Guid productId)
    {
      var product = await _context.Products.AsNoTracking().Where(p => p.ProductId == productId).FirstOrDefaultAsync() ?? throw new InvalidOperationException("Product not found");
      return new ProductOutputDto
      {
        ProductId = product.ProductId,
        Name = product.Name,
        Price = product.Price,
        Description = product.Description,
        CategoryName = product.CategoryName,
        ImageUrl = product.ImageUrl
      };
    }

    public async Task<IEnumerable<ProductOutputDto>> GetProductsAsync()
    {
      var products = await _context.Products.AsNoTracking().ToListAsync();

      return products.Select(p => new ProductOutputDto
      {
        ProductId = p.ProductId,
        Name = p.Name,
        Price = p.Price,
        Description = p.Description,
        CategoryName = p.CategoryName,
        ImageUrl = p.ImageUrl
      }) ?? Enumerable.Empty<ProductOutputDto>();
    }
  }
}
