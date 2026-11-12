using Mango.Services.ProductAPI.API__PL_.Controllers;
using Mango.Services.ProductAPI.Application__BLL_;
using Mango.Services.ProductAPI.Application__BLL_.Contracts;
using Mango.Services.ProductAPI.Data.DbContexts;
using Mango.Services.ProductAPI.Data__DAL_.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

namespace Mango.Services.ProductAPI
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);

      // Add services to the container.

      builder.Services.AddControllers();

      // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
      builder.Services.AddOpenApi();

      builder.Services.AddDbContext<ApplicationDbContext>(options =>
          options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

      builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

      builder.Services.AddScoped<IProductService, ProductService>();
      builder.Services.AddScoped<IProductRepository, ProductRepository>();

      builder.Services.AddOpenApi(options =>
      {
        options.AddDocumentTransformer((document, context, cancellationToken) =>
        {
          document.Info = new OpenApiInfo
          {
            Title = "MSI API",
            Version = "v1",
            Description = "ProductService API by MSI",
            TermsOfService = new Uri("https://example.com/terms"),
            Contact = new OpenApiContact
            {
              Name = "Sergey Miller",
              Email = "s.miller@list.ru",
              Url = new Uri("https://barnaul.hh.ru/applicant/resumes")
            },
            License = new OpenApiLicense
            {
              Name = "Product API LICX",
              Url = new Uri("https://example.com/license")
            }
          };
          return Task.CompletedTask;
        });
      });

      var app = builder.Build();
      
      // Configure the HTTP request pipeline.
      if (app.Environment.IsDevelopment())
      {
        app.MapOpenApi();
        app.MapScalarApiReference();
      }

      app.UseExceptionHandler(opt => {});

      app.UseHttpsRedirection();

      app.UseAuthorization();


      app.MapControllers();

      app.Run();
    }
  }
}
