using Microsoft.AspNetCore.Mvc;

using Ghost.Dtos;
using Ghost.Services.Interfaces;

namespace Ghost.Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class MediaController : Controller
  {
    private readonly IVideoService videoService;

    public MediaController(IVideoService videoService)
    {
      this.videoService = videoService;
    }

    [HttpGet]
    public ActionResult<PageResultDto<VideoDto>> GetVideos(int page = 0, int limit = 10)
    {
      if (page >= 0 && limit > 0)
      {
        return videoService.GetVideos(page, limit);
      }
      else
      {
        return BadRequest();
      }
    }

    [HttpGet("{id}/info")]
    public ActionResult<VideoDto> GetVideoInfo(string id)
    {
      var video = videoService.GetVideoById(id);

      return video;
    }

    [HttpGet("{id}")]
    public ActionResult GetVideo(string id)
    {
      var video = videoService.GetVideoById(id);
      if (video != null)
      {
        return PhysicalFile(video.Path ?? "", "video/mp4", true);
      }
      return NotFound();
    }

    [HttpGet("{id}/thumbnail")]
    public IActionResult GetThumbnail(string id)
    {
      var image = videoService.GenerateThumbnail(id);
      return PhysicalFile(videoService.GenerateThumbnail(id), "image/png", true);
    }
  }
}