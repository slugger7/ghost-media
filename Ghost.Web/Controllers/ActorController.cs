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

    public ActorController(IActorService actorService)
    {
      this.actorService = actorService;
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
  }
}