using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;

using BllProduct = Mango.Services.ProductAPI.Application__BLL_.Models.Product;
using DalProduct = Mango.Services.ProductAPI.Data.Entities.Product;


namespace Mango.Services.ProductAPI.API__PL_.Mappers
{
  public class MappingProfile : Profile
  {
    public MappingProfile()
    {
      CreateMap<BllProduct, DalProduct>().ReverseMap();
    }
  }
}
