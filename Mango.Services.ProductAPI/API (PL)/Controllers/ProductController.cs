using Mango.Services.ProductAPI.API__PL_.DTO;
using Mango.Services.ProductAPI.Application__BLL_.Contracts;
using Microsoft.AspNetCore.Mvc;
using Mango.Services.ProductAPI.Application__BLL_.Models;

namespace Mango.Services.ProductAPI.API__PL_.Controllers
{
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
    public async Task<ResponseDto> AddProductAsync([FromBody] ProductDto product)
    {
      try
      {
        var newProduct = await _productService.CreateProductAsync(new Product
        {
          Name = product.Name,
          Price = product.Price,
          Description = product.Description,
          CategoryName = product.CategoryName,
          ImageUrl = product.ImageUrl
        });
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
    public async Task<ResponseDto> UpdateProductAsyc([FromBody] Product product)
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
