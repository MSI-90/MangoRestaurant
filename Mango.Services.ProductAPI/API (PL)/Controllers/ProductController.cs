using Mango.Services.ProductAPI.Application__BLL_.Contracts;
using Microsoft.AspNetCore.Mvc;

using Mango.Services.ProductAPI.Shared.DTO;

namespace Mango.Services.ProductAPI.API__PL_.Controllers
{
  //TODO: выкинуть паттерн Result (ResponseDto) и использовать IActionResult
  //TODO: Внедрить IExceptionHandlerMiddleware для глобальной обработки ошибок
  //TODO: Внедрить FluentValidation для валидации входных данных, здесь подумать о целесообразности ибо можно обойтись родными DataAnnotations
  //TODO: Exceptions для IExecptionHandlerMiddleware, например NotFoundException и т.д.
  //TODO: Для изображений товаров - облако (здесь подумать что использовать, локально или глобально)

  [Route("api/products")]
  [ApiController]
  public class ProductController : ControllerBase
  {
    private readonly IProductService _productService;
    protected ResponseDto _response;
    public ProductController(IProductService productService)
    {
      _productService = productService;
      this._response = new ResponseDto();
    }

    [HttpGet]
    public async Task<ResponseDto> GetProductsAsync()
    {
      try
      {
        var products = await _productService.GetProductsAsync();
        _response.Result = products;
      }
      catch (Exception ex)
      {
        _response.IsSuccess = false;
        _response.ErrorMessages = new List<string>() { ex.ToString() };
      }
      return _response;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ResponseDto> GetProductByIdAsync(Guid id)
    {
      try
      {
        var product = await _productService.GetProductByIdAsync(id);
        _response.Result = product;
      }
      catch (Exception ex)
      {
        _response.IsSuccess = false;
        _response.ErrorMessages = new List<string>() { ex.ToString() };
      }
      return _response;
    }

    [HttpPost]
    public async Task<ResponseDto> AddProductAsync([FromBody] ProductForCreationDto product)
    {
      try
      {
        var newProduct = await _productService.CreateProductAsync(product);
        _response.Result = newProduct;
      }
      catch (Exception ex)
      {
        _response.IsSuccess = false;
        _response.ErrorMessages = new List<string>() { ex.ToString() };
      }
      return _response;
    }

    [HttpPut]
    public async Task<ResponseDto> UpdateProductAsyc([FromBody] ProductForUpdateDto product)
    {
      try
      {
        var newProduct = await _productService.UpdateProductAsync(product);
        _response.Result = newProduct;
      }
      catch (Exception ex)
      {
        _response.IsSuccess = false;
        _response.ErrorMessages = new List<string>() { ex.ToString() };
      }
      return _response;
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ResponseDto> DeleteProductAsync(Guid id)
    {
      try
      {
        var result = _response.Result = await _productService.DeleteProductByIdAsync(id);
        _response.Result = result;
      }
      catch (Exception ex)
      {
        _response.IsSuccess = false;
        _response.ErrorMessages = new List<string>() { ex.ToString() };
      }
      return _response;
    }
  }
}
