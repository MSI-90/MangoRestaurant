namespace Mango.Services.ProductAPI.Shared.DTO
{
  public record FileDto
  {
    public string? ContentType { get; init; }
    public long Length { get; init; }
    public string? FileName { get; set; }
    public Stream? FileStream { get; init; }
  }
}
