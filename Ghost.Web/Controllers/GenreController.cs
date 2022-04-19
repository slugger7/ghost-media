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
    public ActionResult<List<GenreDto>> GetGenres()
    {
      return genreService.GetGenres();
    }

    [HttpGet("top")]
    public ActionResult<List<GenreDto>> GetTopGenres(int limit = 5, string search = "")
    {
      return genreService.SearchTopGenres(limit, search);
    }
  }
}