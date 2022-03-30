using Ghost.Dtos;
using Ghost.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ghost.Api.Controllers
{
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
    public ActionResult<LibraryDto> CreateLibrary([FromBody] CreateLibraryDto library)
    {
      if (library == null) return BadRequest();
      return libraryService.Create(library.Name ?? "");
    }

    [HttpGet]
    public ActionResult<PageResultDto<LibraryDto>> GetLibraries(int page = 0, int limit = 10)
    {
      return libraryService.GetMany(page, limit);
    }

    [HttpGet("{id}")]
    public ActionResult<LibraryDto> GetLibrary(string id)
    {
      try
      {
        return libraryService.GetDto(id);
      }
      catch (NullReferenceException)
      {
        return NotFound();
      }
    }

    [HttpPut("{id}/add-paths")]
    public ActionResult<LibraryDto> AddFolderToLibrary(string id, [FromBody] AddPathsToLibraryDto pathsLibraryDto)
    {
      try
      {
        return libraryService.AddDirectory(id, pathsLibraryDto);
      }
      catch (NullReferenceException)
      {
        return NotFound();
      }
    }

    [HttpGet("{id}/sync")]
    public ActionResult SyncLibrary(string id)
    {
      try
      {
        libraryService.Sync(id);
        return Ok();
      }
      catch (NullReferenceException)
      {
        return NotFound();
      }
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteLibrary(string id)
    {
      try
      {
        libraryService.Delete(id);

      }
      catch (NullReferenceException)
      {
        return NotFound();
      }


      return Ok();
    }
  }
}