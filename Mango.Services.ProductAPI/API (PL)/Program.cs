using AutoMapper;
using Mango.Services.ProductAPI.API__PL_.Mappers;
using Mango.Services.ProductAPI.Application__BLL_;
using Mango.Services.ProductAPI.Application__BLL_.Contracts;
using Mango.Services.ProductAPI.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

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

      builder.Services.AddAutoMapper(cfg => { }, typeof(Program).Assembly);

      builder.Services.AddScoped<IProductService, ProductService>();

      var app = builder.Build();
      
      // Configure the HTTP request pipeline.
      if (app.Environment.IsDevelopment())
      {
        app.MapOpenApi();
      }

      app.UseSwaggerUI(opt => opt.SwaggerEndpoint("/openapi/v1.json", "v1"));

      app.UseHttpsRedirection();

      app.UseAuthorization();


      app.MapControllers();

      app.Run();
    }
  }
}
