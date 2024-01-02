using Microsoft.AspNetCore.Mvc;
using Ghost.Dtos;
using Ghost.Services;
using Microsoft.AspNetCore.Authorization;
using Ghost.Exceptions;
using System.Security.Claims;

namespace Ghost.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class MediaController : BaseController
{
  private readonly IVideoService videoService;
  private readonly ILogger<MediaController> logger;

  public MediaController(ILogger<MediaController> logger, IVideoService videoService)
  {
    this.logger = logger;
    this.videoService = videoService;
  }

  [HttpGet]
  [Authorize]
  public ActionResult<PageResultDto<VideoDto>> SearchVideos(
    [FromQuery] PageRequestDto pageRequest,
    [FromQuery] FilterQueryDto filters)
  {
    return videoService.SearchVideos(pageRequest, filters, UserId);
  }

  [HttpPut("{id}/title")]
  [Authorize]
  public async Task<ActionResult<VideoDto>> UpdateTitle(int id, [FromBody] TitleUpdateDto titleUpdate)
  {
    try
    {
      return await videoService.UpdateTitle(id, titleUpdate.Title);
    }
    catch (NullReferenceException ex)
    {
      return NotFound(ex.Message);
    }
  }

  [HttpGet("{id}/info")]
  [Authorize]
  public ActionResult<VideoDto> GetVideoInfo(int id)
  {
    var video = videoService.GetVideoInfo(id, UserId);

    return video;
  }

  [HttpPut("{id}/metadata")]
  [Authorize]
  public async Task<ActionResult<VideoDto>> GetVideoMetaData(int id)
  {
    try
    {
      var videoInfo = await videoService.UpdateMetaData(id);
      if (videoInfo == null) return NoContent();
      return videoInfo;
    }
    catch (NullReferenceException ex)
    {
      return NotFound(ex.Message);
    }
  }

  [HttpGet("{id}")]
  public ActionResult GetVideo(int id)
  {
    try
    {
      var video = videoService.GetVideoById(id, UserId, null);
      if (video != null)
      {
        return PhysicalFile(video.Path ?? "", "video/mp4", true);
      }
      return NotFound();
    }
    catch (NullReferenceException ex)
    {
      return NotFound(ex.Message);
    }
  }

  [HttpDelete("{id}")]
  [Authorize]
  public async Task<ActionResult> DeleteVideo(int id)
  {
    try
    {
      await videoService.DeletePermanently(id);
      return NoContent();
    }
    catch (NullReferenceException ex)
    {
      return NotFound(ex.Message);
    }
  }

  [HttpPut("{id}/genres")]
  [Authorize]
  public async Task<ActionResult<VideoDto>> AddGenresByNameToVideo(int id, [FromBody] GenreAddDto genreAddDto)
  {
    try
    {
      return await this.videoService.SetGenresByNameToVideo(id, genreAddDto.Genres);
    }
    catch (NullReferenceException ex)
    {
      return NotFound(ex.Message);
    }
  }

  [HttpGet("genre/{genre}")]
  [Authorize]
  public ActionResult<PageResultDto<VideoDto>> GetVideosForGenre(
    string genre,
    [FromQuery] PageRequestDto pageRequest,
    [FromQuery] FilterQueryDto filters)
  {
    return videoService.GetVideosForGenre(genre, UserId, pageRequest, filters);
  }

  [HttpGet("genre/{genre}/random")]
  [Authorize]
  public ActionResult<VideoDto> GetRandomVideoForGenre(
      string genre,
      [FromQuery] RandomVideoRequestDto randomVideoRequest
  )
  {
    return videoService.GetRandomVideoForGenre(genre, UserId, randomVideoRequest);
  }

  [HttpPut("{id}/actors")]
  [Authorize]
  public async Task<ActionResult<VideoDto>> AddActorsByNameToVideo(int id, [FromBody] ActorAddDto actorAddDto)
  {
    try
    {
      return await this.videoService.SetActorsByNameToVideo(id, actorAddDto.Actors);
    }
    catch (NullReferenceException ex)
    {
      return NotFound(ex.Message);
    }
  }

  [HttpGet("actor/{id}")]
  [Authorize]
  public ActionResult<PageResultDto<VideoDto>> GetVideosForActor(
    int id,
    [FromQuery] PageRequestDto pageRequest,
    [FromQuery] FilterQueryDto filters)
  {
    try
    {
      return videoService.GetVideosForActor(id, UserId, pageRequest, filters);
    }
    catch (NullReferenceException ex)
    {
      return NotFound(ex.Message);
    }
  }

  [HttpGet("actor/{id}/random")]
  [Authorize]
  public ActionResult<VideoDto> GetRandomVideoForActor(
      int id,
      [FromQuery] RandomVideoRequestDto randomVideoRequest
  )
  {
    try
    {
      return videoService.GetRandomVideoForActor(id, UserId, randomVideoRequest);
    }
    catch (NullReferenceException ex)
    {
      return NotFound(ex.Message);
    }
  }

  [HttpPut("{id}/nfo")]
  [Authorize]
  public async Task<ActionResult<VideoDto>> UpdateFromNFO(int id)
  {
    try
    {
      return await videoService.SyncWithNFO(id);
    }
    catch (NullReferenceException ex)
    {
      return NotFound(ex.Message);
    }
  }

  [HttpPut("{id}/chapters")]
  [Authorize]
  public async Task<ActionResult<VideoDto>> GenerateChapters(int id, bool overwrite = false)
  {
    try
    {
      return await videoService.GenerateChapters(id, overwrite);
    }
    catch (NullReferenceException ex)
    {
      return NotFound(ex.Message);
    }
  }

  [HttpPut("{id}/progress")]
  [Authorize]
  public async Task<ActionResult> LogProgress(
    int id,
    [FromBody] ProgressUpdateDto progress)
  {
    try
    {
      await videoService.LogProgress(id, UserId, progress);
      return Ok();
    }
    catch (NullReferenceException ex)
    {
      return NotFound(ex.Message);
    }
  }

  [HttpGet("favourites")]
  [Authorize]
  public ActionResult<PageResultDto<VideoDto>> GetFavourites(
    [FromQuery] PageRequestDto pageRequest,
    [FromQuery] FilterQueryDto filterQueryDto
  )
  {
    try
    {
      return videoService.Favourites(UserId, pageRequest, filterQueryDto);
    }
    catch (NullReferenceException ex)
    {
      return NotFound(ex.Message);
    }
  }

  [HttpGet("favourites/random")]
  [Authorize]
  public ActionResult<VideoDto> GetRandomVideoFromFavourites(
      [FromQuery] RandomVideoRequestDto randomVideoRequest
  )
  {
    try
    {
      return videoService.GetRandomVideoFromFavourites(UserId, randomVideoRequest);
    }
    catch (NullReferenceException ex)
    {
      return NotFound(ex.Message);
    }
  }

  [HttpGet("random")]
  [Authorize]
  public ActionResult<VideoDto> RandomVideo(
      [FromQuery] RandomVideoRequestDto randomVideoRequest
  )
  {
    try
    {
      return videoService.Random(UserId, randomVideoRequest);
    }
    catch (NullReferenceException ex)
    {
      return NotFound(ex.Message);
    }
  }

  [HttpPut("{id}/relations/{relateTo}")]
  [Authorize]
  public async Task<ActionResult<List<VideoDto>>> RelateVideo(int id, int relateTo)
  {
    try
    {
      return await videoService.RelateVideo(id, relateTo);
    }
    catch (NullReferenceException ex)
    {
      return NotFound(ex.Message);
    }
    catch (VideoRelationException ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [HttpDelete("{id}/relations/{relatedTo}")]
  [Authorize]
  public async Task<ActionResult<List<VideoDto>>> DeleteRelation(int id, int relatedTo)
  {
    try
    {
      return await videoService.DeleteRelation(id, relatedTo);
    }
    catch (NullReferenceException ex)
    {
      return NotFound(ex.Message);
    }
  }

  [HttpPost("{id}/sub-video")]
  [Authorize]
  public async Task<ActionResult> CreateSubVideo(
      int id,
      [FromBody] SubVideoRequestDto subVideoRequest)
  {
    if (subVideoRequest.EndMillis <= subVideoRequest.StartMillis)
    {
      return BadRequest("End is smaller than start");
    }
    try
    {
      await videoService.CreateSubVideo(id, UserId, subVideoRequest);
      return Ok();
    }
    catch (NullReferenceException ex)
    {
      return NotFound(ex.Message);
    }
  }

  [HttpPost("{id}/convert")]
  public async Task<ActionResult> ConvertVideo(int id, [FromBody] ConvertRequestDto convertRequest)
  {
    try
    {
      await videoService.Convert(id, convertRequest);

      return Ok();
    }
    catch (FileExistsException)
    {
      return BadRequest("File already exists");
    }
  }
}