using Ghost.Dtos;
using Ghost.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ghost.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ActorController : BaseController
{
  private readonly IActorService actorService;
  private readonly ILogger<ActorController> logger;

  public ActorController(IActorService actorService, ILogger<ActorController> logger)
  {
    this.actorService = actorService;
    this.logger = logger;
  }

  [HttpGet("{name}")]
  [Authorize]
  public ActionResult<ActorDto> GetActorByName(string name)
  {
    try
    {
      return actorService.GetActorByName(name, UserId);
    }
    catch (NullReferenceException)
    {
      return NotFound();
    }
  }

  [HttpGet]
  [Authorize]
  public ActionResult<List<ActorDto>> GetActors()
  {
    return actorService.GetActors(UserId);
  }

  [HttpGet("favourites")]
  [Authorize]
  public ActionResult<List<ActorDto>> GetFavouriteActors()
  {
    return actorService.GetFavouriteActors(UserId);
  }

  [HttpGet("video/{videoId}")]
  [Authorize]
  public ActionResult<List<ActorDto>> GetActorsForVideo(int videoId)
  {
    try
    {
      return actorService.GetActorsForVideo(videoId, UserId);
    }
    catch (NullReferenceException ex)
    {
      logger.LogWarning("Actor for video failed:u {0}", ex.Message);
      return NotFound();
    }
  }

  [HttpPut("{id}")]
  [Authorize]
  public async Task<ActionResult<ActorDto>> UpdateName(int id, [FromBody] ActorNameUpdateDto actorNameUpdateDto)
  {
    try
    {
      return await this.actorService.UpdateName(id, actorNameUpdateDto.Name);
    }
    catch (NullReferenceException)
    {
      return NotFound();
    }
  }
}