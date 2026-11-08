namespace Mango.Services.ProductAPI.API__PL_.DTO
{
  public class ResponseDto<T>
  {
    public bool IsSuccess { get; set; } = true;
    public T? Result { get; set; }
    public string? DisplayMessage { get; set; } = string.Empty;
    public List<string>? ErrorMessages { get; set; }
  }
}
