using Ghost.Dtos;
using Ghost.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ghost.Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ImageController : Controller
  {
    private readonly ILogger<ImageController> logger;
    private readonly IImageService imageService;

    public ImageController(
      ILogger<ImageController> logger,
      IImageService imageService
    )
    {
      this.logger = logger;
      this.imageService = imageService;
    }

    [HttpGet]
    public ActionResult<PageResultDto<ImageDto>> GetImages([FromQuery] PageRequestDto pageRequest)
    {
      return imageService.GetImages(pageRequest);
    }

    [HttpGet("{id}/info")]
    public ActionResult<ImageDto> GetImageInfo(int id)
    {
      logger.LogDebug("Getting image info {0}", id);
      try
      {
        return imageService.GetImage(id);
      }
      catch (NullReferenceException)
      {
        return NotFound();
      }
    }

    [HttpGet("{id}/{title}")]
    public ActionResult GetImageFile(int id)
    {
      logger.LogDebug("Getting image {0}", id);
      try
      {
        var image = imageService.GetImage(id);
        return PhysicalFile(image.Path, "image/png", true);
      }
      catch (NullReferenceException)
      {
        return NotFound();
      }
    }
  }
}