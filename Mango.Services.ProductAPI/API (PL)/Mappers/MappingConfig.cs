using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;

using EntityProduct = Mango.Services.ProductAPI.Application__BLL_.Models.Product;
using ServiceProduct = Mango.Services.ProductAPI.Data.Entities.Product;


namespace Mango.Services.ProductAPI.API__PL_.Mappers
{
  public class MappingConfig
  {
    public static MapperConfiguration RegisterMaps()
    {
      var mappingConfig = new MapperConfiguration(config =>
      {
        config.CreateMap<EntityProduct, ServiceProduct>().ReverseMap();
      }, NullLoggerFactory.Instance);

      return mappingConfig;
    }
  }
}
