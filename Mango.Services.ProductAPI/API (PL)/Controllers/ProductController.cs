using Mango.Services.ProductAPI.Application__BLL_.Contracts;
using Microsoft.AspNetCore.Mvc;

using Mango.Services.ProductAPI.Shared.DTO;

namespace Mango.Services.ProductAPI.API__PL_.Controllers
{
  //TODO: Внедрить IExceptionHandlerMiddleware для глобальной обработки ошибок
  //TODO: Внедрить FluentValidation для валидации входных данных, здесь подумать о целесообразности ибо можно обойтись родными DataAnnotations
  //TODO: Exceptions для IExecptionHandlerMiddleware, например NotFoundException и т.д.
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

    [HttpGet]
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
    public async Task<IActionResult> UpdateProductAsyc([FromBody] ProductForUpdateDto product)
    {
      var newProduct = await _productService.UpdateProductAsync(product);
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
