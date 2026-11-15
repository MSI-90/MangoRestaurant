namespace Mango.Services.ProductAPI.Shared.DTO
{
  public record S3StorageDto
  {
    public string AccessKey { get; init; } = string.Empty;
    public string SecretAccessKey { get; init; } = string.Empty;
    public string EndpointURL { get; init; } = string.Empty;
    public string BucketName { get; init; } = string.Empty;
  }
}