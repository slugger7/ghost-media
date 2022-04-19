using Ghost.Dtos;
using Ghost.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ghost.Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class GenreController : Controller
  {
    private readonly IGenreService genreService;

    public GenreController(IGenreService genreService)
    {
      this.genreService = genreService;
    }

    [HttpGet("{name}")]
    public ActionResult<GenreViewDto> GetGenreByName(string name)
    {
      try
      {
        return this.genreService.GetGenreByName(name);
      }
      catch (NullReferenceException)
      {
        return NotFound();
      }
    }

    [HttpGet]
    public ActionResult<PageResultDto<GenreDto>> GetGenres(int page = 0, int limit = 12)
    {
      if (page >= 0 && limit > 0)
      {
        return genreService.GetGenres(page, limit);
      }
      else
      {
        return BadRequest();
      }
    }
  }
}