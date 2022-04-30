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

    [HttpGet("{id}")]
    public ActionResult<ImageDto> GetImage(int id)
    {
      try
      {
        return imageService.GetImage(id);
      }
      catch (NullReferenceException)
      {
        return NotFound();
      }
    }
  }
}