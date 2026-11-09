using Mango.Services.ProductAPI.Application__BLL_.Exceptions;
using Mango.Services.ProductAPI.Application__BLL_.Models.ErrorModel;
using Microsoft.AspNetCore.Diagnostics;

namespace Mango.Services.ProductAPI.Application__BLL_
{
  public class GlobalExceptionHandler : IExceptionHandler
  {
    private readonly ILogger<GlobalExceptionHandler> _logger;
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
      _logger = logger;
    }
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
      httpContext.Response.ContentType = "application/json";

      var contextFeature = httpContext.Features.Get<IExceptionHandlerFeature>();
      if (contextFeature is not null)
      {
        switch (contextFeature.Error)
        {
          case ProductNotFoundException:
            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            break;
          default:
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            break;
        }

        _logger.LogError($"Что-то пошло не так: {exception.Message}");
        await httpContext.Response.WriteAsync(new ErrorDetails()
        {
          StatusCode = httpContext.Response.StatusCode,
          Message = exception.Message,
        }.ToString());
      }

      return true;
    }
  }
}
