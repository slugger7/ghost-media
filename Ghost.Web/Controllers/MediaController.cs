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
    public ActionResult<PageResultDto<VideoDto>> SearchVideos(
      [FromQuery] PageRequestDto pageRequest,
      [FromHeader(Name = "User-Id")] int userId)
    {
      return videoService.SearchVideos(pageRequest, userId);
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
    public ActionResult<VideoDto> GetVideoInfo(int id, [FromHeader(Name = "User-Id")] int userId)
    {
      var video = videoService.GetVideoById(id, userId, new List<string> { "VideoImages.Image", "Chapters.Image", "FavouritedBy.User" });

      return video;
    }

    [HttpPut("{id}/metadata")]
    public async Task<ActionResult<VideoDto>> GetVideoMetaData(int id)
    {
      try
      {
        var videoInfo = await videoService.UpdateMetadData(id);
        if (videoInfo == null) return NoContent();
        return videoInfo;
      }
      catch (NullReferenceException)
      {
        return NotFound();
      }
    }

    [HttpGet("{id}")]
    public ActionResult GetVideo(int id, [FromHeader(Name = "User-Id")] int userId)
    {
      try
      {
        var video = videoService.GetVideoById(id, userId, null);
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

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteVideo(int id)
    {
      try
      {
        await videoService.DeletePermanently(id);
        return NoContent();
      }
      catch (NullReferenceException)
      {
        logger.LogWarning("Video was not found: {id}", id);
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
      try
      {
        return await videoService.SyncWithNFO(id);
      }
      catch (NullReferenceException)
      {
        logger.LogWarning("Video was not found when updating from NFO {0}", id);
        return NotFound();
      }
    }

    [HttpPut("{id}/chapters")]
    public async Task<ActionResult<VideoDto>> GenerateChapters(int id, bool overwrite = false)
    {
      try
      {
        return await videoService.GenerateChapters(id, overwrite);
      }
      catch (NullReferenceException)
      {
        logger.LogWarning("Video was not found when generating chapters {0}", id);
        return NotFound();
      }
    }
  }
}