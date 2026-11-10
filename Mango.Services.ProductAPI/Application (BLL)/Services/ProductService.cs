using Mango.Services.ProductAPI.Application__BLL_.Contracts;
using Mango.Services.ProductAPI.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

using EntityProduct = Mango.Services.ProductAPI.Data.Entities.Product;
using Mango.Services.ProductAPI.Shared.DTO;
using Mango.Services.ProductAPI.Application__BLL_.Exceptions;

namespace Mango.Services.ProductAPI.Application__BLL_
{
  public class ProductService : IProductService
  {
    private readonly ApplicationDbContext _context;
    public ProductService(ApplicationDbContext context)
    {
      _context = context;
    }
    public async Task<ProductOutputDto?> CreateProductAsync(ProductForCreationDto productForCreation)
    {
      var newProduct = TranslateToEntityProduct(productForCreation);
      var result = _context.Products.Add(newProduct!);
      await _context.SaveChangesAsync();
      return TranslateToProductOutput(result.Entity);
    }
    
    public async Task<ProductOutputDto?> UpdateProductAsync(Guid productId, ProductForUpdateDto product)
    {
      await GetProductByIdAsync(productId);
      _context.Products.Update(new EntityProduct
      {
        ProductId = productId,
        Name = product.Name,
        Price = product.Price,
        Description = product.Description,
        CategoryName = product.CategoryName,
        ImageUrl = product.ImageUrl
      });

      await _context.SaveChangesAsync();
      return await GetProductByIdAsync(productId);
    }

    public async Task<bool> DeleteProductByIdAsync(Guid productId)
    {
      var productInDb = await GetProductByIdAsync(productId);
      _context.Products.Remove(new EntityProduct { ProductId = productId });

      await _context.SaveChangesAsync();
      return true;
    } 

    public async Task<ProductOutputDto?> GetProductByIdAsync(Guid productId)
    {
      var product = await _context.Products.AsNoTracking().Where(p => p.ProductId == productId).FirstOrDefaultAsync();
      return product is not null 
        ? TranslateToProductOutput(product) 
        : throw new ProductNotFoundException(productId);
    }

    public async Task<IEnumerable<ProductOutputDto?>> GetProductsAsync()
    {
      var products = await _context.Products.AsNoTracking().ToListAsync();
      return products.Select(p => TranslateToProductOutput(p)) ?? [];
    }

    private EntityProduct? TranslateToEntityProduct(ProductForCreationDto? productForCreation) =>
      productForCreation is not null ? new EntityProduct
      {
        ProductId = Guid.NewGuid(),
        Name = productForCreation.Name,
        Price = productForCreation.Price,
        Description = productForCreation.Description,
        CategoryName = productForCreation.CategoryName,
        ImageUrl = productForCreation.ImageUrl
      } : null;

    private ProductOutputDto? TranslateToProductOutput(EntityProduct? product) =>
      product is not null ? new ProductOutputDto
      {
        ProductId = product.ProductId,
        Name = product.Name,
        Price = product.Price,
        Description = product.Description,
        CategoryName = product.CategoryName,
        ImageUrl = product.ImageUrl
      } : null;
  }
}
