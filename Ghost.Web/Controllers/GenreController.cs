using Ghost.Dtos;
using Ghost.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ghost.Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class GenreController : Controller
  {
    private readonly IGenreService genreService;
    private readonly ILogger<GenreController> logger;

    public GenreController(IGenreService genreService, ILogger<GenreController> logger)
    {
      this.genreService = genreService;
      this.logger = logger;
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
      return genreService.SearchTopGenres(search, limit);
    }

    [HttpGet("video/{videoId}")]
    public ActionResult<List<GenreDto>> GetGenresForVideo(int videoId)
    {
      try
      {
        return genreService.GetGenresForVideo(videoId);
      }
      catch (NullReferenceException ex)
      {
        logger.LogWarning("Genre for video failed", ex);
        return NotFound();
      }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<GenreViewDto>> UpdateGenreName(int id, [FromBody] GenreUpdateDto genreUpdate)
    {
      try
      {
        return await genreService.UpdateGenre(id, genreUpdate.Name);
      }
      catch (NullReferenceException)
      {
        return NotFound();
      }
    }
  }
}