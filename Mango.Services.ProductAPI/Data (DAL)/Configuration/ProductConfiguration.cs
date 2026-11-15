using Mango.Services.ProductAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mango.Services.ProductAPI.Data__DAL_.Configuration
{
  public class ProductConfiguration : IEntityTypeConfiguration<Product>
  {
    public void Configure(EntityTypeBuilder<Product> builder)
    {
      builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
      builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
      builder.Property(p => p.Description).HasMaxLength(5000);
      builder.Property(p => p.CategoryName).HasMaxLength(150);
      builder.Property(p => p.ImageUrl);

      builder.ToTable(t => t.HasCheckConstraint(
        "CK_Product_Price",
        "[Price] >= 1 AND [Price] <= 1000000"
      ));
    }
  }
}
