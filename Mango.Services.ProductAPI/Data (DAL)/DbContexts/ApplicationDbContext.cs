using Mango.Services.ProductAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.Data.DbContexts
{
  public class ApplicationDbContext : DbContext
  {
    public DbSet<Product> Products { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
  }
}
