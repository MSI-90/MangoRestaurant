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

    [HttpGet]
    [Route("{id:guid}", Name = "GetProductById")]
    public async Task<IActionResult> GetProductByIdAsync(Guid id)
    {
      var product = await _productService.GetProductByIdAsync(id);
      return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> AddProductAsync([FromBody] ProductForCreationDto product)
    {
      var newProduct = await _productService.CreateProductAsync(product);
      return CreatedAtRoute("GetProductById", new { id = newProduct}, newProduct);
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> UpdateProductAsyc(Guid id, [FromBody] ProductForUpdateDto product)
    {
      var newProduct = await _productService.UpdateProductAsync(id, product);
      return Ok(newProduct);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> DeleteProductAsync(Guid id)
    {
      var result = await _productService.DeleteProductByIdAsync(id);
      return NoContent();
    }
  }
}
