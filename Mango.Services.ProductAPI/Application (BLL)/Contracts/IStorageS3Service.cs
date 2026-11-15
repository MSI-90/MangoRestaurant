using Mango.Services.ProductAPI.Shared.DTO;

namespace Mango.Services.ProductAPI.Application__BLL_.Contracts
{
  public interface IStorageS3Service
  {
    Task<bool> CheckBucketAsync();
    Task<(bool, string?, string?)> UploadFileAsync(FileDto? file);
    string GeneratePresignedURL(string objectKey, double duration);
    Task DeletePhotoAsync(Guid? photoId);
  }
}
