using Mango.Services.ProductAPI.Application__BLL_.Contracts;
using Mango.Services.ProductAPI.Shared.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.API__PL_.Controllers
{
  [Route("api/photo")]
  [ApiController]
  public class PhotoController : ControllerBase
  {
    private readonly IPhotoService _photoService;
    public PhotoController(IPhotoService photoService)
    {
      _photoService = photoService;
    }

    [HttpPost]
    public async Task<IActionResult> AddPhoto(IFormFile? file)
    {
      FileDto? fileDto = null;
      if (file is not null)
      {
        fileDto = new FileDto
        {
          ContentType = file.ContentType,
          Length = file.Length,
          FileName = file.FileName,
          FileStream = file.OpenReadStream()
        };
      }
      var photoId = await _photoService.GetPhotoResumeAync(fileDto);
      return Ok(photoId);
    }
  }
}
