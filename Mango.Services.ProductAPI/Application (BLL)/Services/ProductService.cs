using AutoMapper;
using Mango.Services.ProductAPI.Application__BLL_.Contracts;
using Mango.Services.ProductAPI.Application__BLL_.Models;
using Mango.Services.ProductAPI.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

using ServiceProduct = Mango.Services.ProductAPI.Application__BLL_.Models.Product;
using EntityProduct = Mango.Services.ProductAPI.Data.Entities.Product;

namespace Mango.Services.ProductAPI.Application__BLL_
{
  public class ProductService : IProductService
  {
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    public ProductService(ApplicationDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }
    public async Task<Guid> CreateProductAsync(Product product)
    {
      var newProduct = _mapper.Map<EntityProduct>(product) ?? throw new ArgumentNullException(nameof(product));

      newProduct.ProductId = Guid.NewGuid();
      var result = await _context.Products.AddAsync(newProduct);
      _context.SaveChanges();
      return result.Entity.ProductId;
    }

    public async Task<ServiceProduct> UpdateProductAsync(ServiceProduct product)
    {
      var productInDb = await GetProductByIdAsync(product.ProductId) ?? throw new InvalidOperationException("Product not found");
      var productToUpdate = _mapper.Map<EntityProduct>(productInDb);

      _context.Products.Update(productToUpdate);
      await _context.SaveChangesAsync();

      return await GetProductByIdAsync(product.ProductId);
    }

    public async Task<bool> DeleteProductByIdAsync(Guid productId)
    {
      var product = await GetProductByIdAsync(productId);
      if (product is null) return false;

      _context.Products.Remove(_mapper.Map<EntityProduct>(product));
      await _context.SaveChangesAsync();
      return true;
    } 

    public async Task<Product> GetProductByIdAsync(Guid productId)
    {
      var product = await _context.Products.AsNoTracking().Where(p => p.ProductId == productId).FirstOrDefaultAsync() ?? throw new InvalidOperationException("Product not found");
      return _mapper.Map<ServiceProduct>(product);
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
      var products = await _context.Products.AsNoTracking().ToListAsync();
      return _mapper.Map<IEnumerable<ServiceProduct>>(products);
    }
  }
}
