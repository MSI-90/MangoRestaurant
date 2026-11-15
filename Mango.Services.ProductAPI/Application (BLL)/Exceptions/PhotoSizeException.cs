namespace Mango.Services.ProductAPI.Application__BLL_.Exceptions
{
  public class PhotoSizeException : Exception
  {
    public PhotoSizeException() : base("Файл отсутствует, либо размер файла слишком велик.")
    {
    }
  }
}
