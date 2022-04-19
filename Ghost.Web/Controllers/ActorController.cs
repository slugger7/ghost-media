using Ghost.Dtos;
using Ghost.Services.Interfaces;
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
    public ActionResult<ActorDto> GetGenreByName(string name)
    {
      try
      {
        return actorService.GetGenreByName(name);
      }
      catch (NullReferenceException)
      {
        return NotFound();
      }
    }

    [HttpGet]
    public ActionResult<List<ActorDto>> GetGenres()
    {
      return actorService.GetActors();
    }
  }
}