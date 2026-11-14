namespace Mango.Services.ProductAPI.Shared.DTO
{
  public record S3StorageDto
    (string? AccessKey, 
    string? SecretAccessKey, 
    string? EndpointURL, 
    string? BucketName);
}
