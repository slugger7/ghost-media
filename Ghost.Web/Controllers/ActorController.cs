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
    public ActionResult<ActorDto> GetActorByName(string name)
    {
      try
      {
        return actorService.GetActorByName(name);
      }
      catch (NullReferenceException)
      {
        return NotFound();
      }
    }

    [HttpGet]
    public ActionResult<List<ActorDto>> GetActors()
    {
      return actorService.GetActors();
    }

    [HttpGet("video/{videoId}")]
    public ActionResult<List<ActorDto>> GetActorsForVideo(int videoId)
    {
      try
      {
        return actorService.GetActorsForVideo(videoId);
      }
      catch (NullReferenceException ex)
      {
        logger.LogWarning("Actor for video failed", ex);
        return NotFound();
      }
    }
  }
}