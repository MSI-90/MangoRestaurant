using Mango.Services.ProductAPI.Application__BLL_.Contracts;
using Mango.Services.ProductAPI.Application__BLL_.Exceptions;
using Mango.Services.ProductAPI.Shared.DTO;

namespace Mango.Services.ProductAPI.Application__BLL_.Services
{
  public sealed class PhotoService : IPhotoService
  {
    private readonly IStorageS3Service _s3;
    public PhotoService(IStorageS3Service s3)
    {
      _s3 = s3;
    }
    private bool CheckFileOnValidAsync(FileDto? file)
    {
      if (file?.Length == 0 || file?.Length > 3000000)
        return false;

      var allowedTypes = new[] { "image/png", "image/jpeg", "image/jpg" };
      if (!allowedTypes.Contains(file?.ContentType))
        return false;

      return true;
    }
    public async Task<PhotoDto?> GetPhotoResumeAync(FileDto? file)
    {
      var checkFile = CheckFileOnValidAsync(file);
      if (!checkFile)
        throw new PhotoSizeException();

      var putPhotoOnBucket = await _s3.UploadFileAsync(file);

      PhotoDto? newPhoto = null;
      if (putPhotoOnBucket.Item1)
        newPhoto = new PhotoDto(new Guid(putPhotoOnBucket.Item2 ?? ""), putPhotoOnBucket.Item3);

      return newPhoto;
    }
  }
}
