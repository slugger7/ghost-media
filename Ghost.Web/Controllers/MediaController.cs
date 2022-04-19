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
    public ActionResult<PageResultDto<VideoDto>> GetVideos(int page = 0, int limit = 12)
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

    [HttpGet("{id}/metadata")]
    public ActionResult<VideoMetaDataDto> GetVideoMetaData(string id)
    {
      try
      {
        var videoInfo = videoService.GetVideoMetaData(id);
        if (videoInfo == null) return NoContent();
        return videoInfo;
      }
      catch (NullReferenceException)
      {
        return NotFound();
      }
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
      try
      {
        var image = videoService.GenerateThumbnail(id);
        return PhysicalFile(videoService.GenerateThumbnail(id), "image/png", true);
      }
      catch (NullReferenceException)
      {
        return NotFound();
      }
    }

    [HttpPut("{id}/genres")]
    public ActionResult<VideoDto> AddGenresByNameToVideo(string id, [FromBody] GenreAddDto genreAddDto)
    {
      try
      {
        return this.videoService.AddGenresByNameToVideo(id, genreAddDto.Genres);
      }
      catch (NullReferenceException)
      {
        return NotFound();
      }
    }

    [HttpGet("genre/{genre}")]
    public ActionResult<PageResultDto<VideoDto>> GetVideosForGenre(string genre, int page = 0, int limit = 12)
    {
      return videoService.GetVideosForGenre(genre, page, limit);
    }

    [HttpPut("{id}/actors")]
    public ActionResult<VideoDto> AddActorsByNameToVideo(string id, [FromBody] ActorAddDto actorAddDto)
    {
      try
      {
        return this.videoService.AddActorsByNameToVideo(id, actorAddDto.Actors);
      }
      catch (NullReferenceException)
      {
        return NotFound();
      }
    }
  }
}