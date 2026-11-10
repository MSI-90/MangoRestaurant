using Mango.Services.ProductAPI.Application__BLL_.Contracts;
using Microsoft.AspNetCore.Mvc;

using Mango.Services.ProductAPI.Shared.DTO;

namespace Mango.Services.ProductAPI.API__PL_.Controllers
{
  //TODO: Внедрить FluentValidation для валидации входных данных, здесь подумать о целесообразности ибо можно обойтись родными DataAnnotations
  //TODO: Для изображений товаров - облако (здесь подумать что использовать, локально или глобально)

  [Route("api/products")]
  [ApiController]
  public class ProductController : ControllerBase
  {
    private readonly IProductService _productService;
    public ProductController(IProductService productService)
    {
      _productService = productService;
    }

    /// <summary>
    /// Получает список всех продуктов
    /// </summary>
    /// <returns></returns>
    /// <response code="200">Возвращает список продуктов</response>
    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetProductsAsync()
    {
      var products = await _productService.GetProductsAsync();
      return Ok(products);
    }

    /// <summary>
    /// Получить продукт по идентификатору
    /// </summary>
    /// <param name="productId">GUID товара</param>
    /// <returns>Модель продукта</returns>
    [HttpGet]
    [Route("{productId:guid}", Name = "GetProductById")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetProductByIdAsync(Guid productId)
    {
      var product = await _productService.GetProductByIdAsync(productId);
      return Ok(product);
    }

    /// <summary>
    /// Добавить новый продукт
    /// </summary>
    /// <param name="product">Модель продукта для создания</param>
    /// <returns>Созданный продукт</returns>
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> AddProductAsync([FromBody] ProductForCreationDto product)
    {
      var newProduct = await _productService.CreateProductAsync(product);
      return CreatedAtRoute("GetProductById", new { productId = newProduct?.ProductId}, newProduct);
    }

    /// <summary>
    /// Обновить полностью существующий продукт
    /// </summary>
    /// <param name="productId">Guid продукта</param>
    /// <param name="product">Модель продукта для обновления</param>
    /// <returns>Обновленный продукт</returns>
    [HttpPut]
    [Route("{productId:guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateProductAsyc(Guid productId, [FromBody] ProductForUpdateDto product)
    {
      var newProduct = await _productService.UpdateProductAsync(productId, product);
      return Ok(newProduct);
    }

    /// <summary>
    /// Удалить продукт по идентификатору
    /// </summary>
    /// <param name="productId">Guid продукта</param>
    /// <returns>204 статус</returns>
    [HttpDelete]
    [Route("{productId:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteProductAsync(Guid productId)
    {
      var result = await _productService.DeleteProductByIdAsync(productId);
      return NoContent();
    }
  }
}
