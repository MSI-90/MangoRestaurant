using Mango.Services.ProductAPI.Shared.DTO;

namespace Mango.Services.ProductAPI.Application__BLL_.Contracts
{
  public interface IPhotoService
  {
    Task<PhotoDto?> GetPhotoResumeAync(FileDto? file);
  }
}
