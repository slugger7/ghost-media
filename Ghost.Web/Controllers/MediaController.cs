using Microsoft.AspNetCore.Mvc;

using Ghost.Dtos;
using Ghost.Services;

namespace Ghost.Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class MediaController : Controller
  {
    private readonly IVideoService videoService;
    private readonly ILogger<MediaController> logger;

    public MediaController(ILogger<MediaController> logger, IVideoService videoService)
    {
      this.logger = logger;
      this.videoService = videoService;
    }

    [HttpGet]
    public ActionResult<PageResultDto<VideoDto>> SearchVideos([FromQuery] PageRequestDto pageRequest)
    {
      return videoService.SearchVideos(pageRequest);
    }

    [HttpPut("{id}/title")]
    public async Task<ActionResult<VideoDto>> UpdateTitle(int id, [FromBody] TitleUpdateDto titleUpdate)
    {
      try
      {
        return await videoService.UpdateTitle(id, titleUpdate.Title);
      }
      catch (NullReferenceException)
      {
        return NotFound();
      }
    }

    [HttpGet("{id}/info")]
    public ActionResult<VideoDto> GetVideoInfo(int id)
    {
      var video = videoService.GetVideoById(id);

      return video;
    }

    [HttpGet("{id}/metadata")]
    public ActionResult<VideoMetaDataDto> GetVideoMetaData(int id)
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
    public ActionResult GetVideo(int id)
    {
      try
      {
        var video = videoService.GetVideoById(id);
        if (video != null)
        {
          return PhysicalFile(video.Path ?? "", "video/mp4", true);
        }
        return NotFound();
      }
      catch (NullReferenceException)
      {
        return NotFound();
      }
    }

    [HttpGet("{id}/thumbnail")]
    public IActionResult GetThumbnail(int id)
    {
      try
      {
        return Ok();
        //return PhysicalFile(videoService.GenerateThumbnail(id), "image/png", true);
      }
      catch (NullReferenceException)
      {
        return NotFound();
      }
    }

    [HttpPut("{id}/genres")]
    public ActionResult<VideoDto> AddGenresByNameToVideo(int id, [FromBody] GenreAddDto genreAddDto)
    {
      try
      {
        return this.videoService.SetGenresByNameToVideo(id, genreAddDto.Genres);
      }
      catch (NullReferenceException)
      {
        return NotFound();
      }
    }

    [HttpGet("genre/{genre}")]
    public ActionResult<PageResultDto<VideoDto>> GetVideosForGenre(string genre, [FromQuery] PageRequestDto pageRequest)
    {
      return videoService.GetVideosForGenre(genre, pageRequest);
    }

    [HttpPut("{id}/actors")]
    public ActionResult<VideoDto> AddActorsByNameToVideo(int id, [FromBody] ActorAddDto actorAddDto)
    {
      try
      {
        return this.videoService.SetActorsByNameToVideo(id, actorAddDto.Actors);
      }
      catch (NullReferenceException)
      {
        return NotFound();
      }
    }

    [HttpGet("actor/{id}")]
    public ActionResult<PageResultDto<VideoDto>> GetVideosForActor(int id, [FromQuery] PageRequestDto pageRequest)
    {
      try
      {
        return videoService.GetVideosForActor(id, pageRequest);
      }
      catch (NullReferenceException)
      {
        return NotFound();
      }
    }

    [HttpPut("{id}/nfo")]
    public async Task<ActionResult<VideoDto>> UpdateFromNFO(int id)
    {
      return await videoService.SyncWithNFO(id);
    }
  }
}