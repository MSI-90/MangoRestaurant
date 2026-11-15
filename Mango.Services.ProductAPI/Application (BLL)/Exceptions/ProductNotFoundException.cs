namespace Mango.Services.ProductAPI.Application__BLL_.Exceptions
{
  public class ProductNotFoundException(Guid productId) : BaseNotFoundException($"Продукт с id: {productId} не существует в базе данных")
  {
  }
}
