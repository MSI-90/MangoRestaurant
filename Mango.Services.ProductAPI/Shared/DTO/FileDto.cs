namespace Mango.Services.ProductAPI.Shared.DTO
{
  public record FileDto(
    string? ContentType,
    long Length,
    string? FileName,
    Stream? FileStream);
}
