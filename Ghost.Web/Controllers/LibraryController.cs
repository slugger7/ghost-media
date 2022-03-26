using Ghost.Dtos;
using Ghost.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ghost.Api.Controllers
{
  public class Thing {
    public string? Name {get;set;}
  }

  [ApiController]
  [Route("api/[controller]")]
  public class LibraryController : Controller
  {
    private readonly ILibraryService libraryService;

    public LibraryController(ILibraryService libraryService)
    {
      this.libraryService = libraryService;
    }

    [HttpPost]
    public ActionResult<LibraryDto> CreateLibrary([FromBody] Thing thing)
    {
      if (thing == null) return BadRequest();
      return libraryService.CreateLibrary(thing.Name ?? "");
    }

    [HttpGet]
    public ActionResult<PageResultDto<LibraryDto>> GetLibraries()
    {
      return libraryService.GetLibraries();
    }
  }
}