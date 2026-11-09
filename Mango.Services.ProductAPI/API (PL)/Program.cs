using Mango.Services.ProductAPI.API__PL_.Controllers;
using Mango.Services.ProductAPI.Application__BLL_;
using Mango.Services.ProductAPI.Application__BLL_.Contracts;
using Mango.Services.ProductAPI.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

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
      //builder.Services.AddOpenApi();

      builder.Services.AddDbContext<ApplicationDbContext>(options =>
          options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

      builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

      builder.Services.AddScoped<IProductService, ProductService>();

      builder.Services.AddSwaggerGen(options =>
      {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
          Title = "MSI API",
          Version = "v1",
          Description = "ProductService API by MSI",
          TermsOfService = new Uri("https://example.com/terms"),
          Contact = new OpenApiContact
          {
            Name = "Sergey Miller",
            Email = "s.miller@list.ru",
            Url = new Uri(@"https://barnaul.hh.ru/applicant/resumes?hhtmFrom=main&hhtmFromLabel=header")
          },
          License = new OpenApiLicense
          {
            Name = "Product API LICX",
            Url = new Uri("https://example.com/license")
          }
        });

        // XML-документация
        var xmlFile = $"{typeof(ProductController).Assembly.GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

        options.IncludeXmlComments(xmlPath);
      });


      var app = builder.Build();
      
      // Configure the HTTP request pipeline.
      if (app.Environment.IsDevelopment())
      {
        app.MapOpenApi();
      }

      app.UseSwagger();
      app.UseSwaggerUI(s => s.SwaggerEndpoint("/swagger/v1/swagger.json", "MSI API v1"));

      app.UseExceptionHandler(opt => {});

      app.UseHttpsRedirection();

      app.UseAuthorization();


      app.MapControllers();

      app.Run();
    }
  }
}
