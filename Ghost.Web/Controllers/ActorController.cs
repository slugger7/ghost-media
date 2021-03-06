using Ghost.Dtos;
using Ghost.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ghost.Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ActorController : Controller
  {
    private readonly IActorService actorService;
    private readonly ILogger<ActorController> logger;

    public ActorController(IActorService actorService, ILogger<ActorController> logger)
    {
      this.actorService = actorService;
      this.logger = logger;
    }

    [HttpGet("{name}")]
    public ActionResult<ActorDto> GetActorByName(string name, [FromHeader(Name = "User-Id")] int userId)
    {
      try
      {
        return actorService.GetActorByName(name, userId);
      }
      catch (NullReferenceException)
      {
        return NotFound();
      }
    }

    [HttpGet]
    public ActionResult<List<ActorDto>> GetActors([FromHeader(Name = "User-Id")] int userId)
    {
      return actorService.GetActors(userId);
    }

    [HttpGet("video/{videoId}")]
    public ActionResult<List<ActorDto>> GetActorsForVideo(int videoId, [FromHeader(Name = "User-Id")] int userId)
    {
      try
      {
        return actorService.GetActorsForVideo(videoId, userId);
      }
      catch (NullReferenceException ex)
      {
        logger.LogWarning("Actor for video failed", ex);
        return NotFound();
      }
    }

    [HttpPut("{id}")]
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
}