using Mango.Services.ProductAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Mango.Services.ProductAPI.Data.DbContexts
{
  public class ApplicationDbContext : DbContext
  {
    public DbSet<Product> Products { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfigurationsFromAssembly(
        Assembly.GetExecutingAssembly());
    }
  }
}
