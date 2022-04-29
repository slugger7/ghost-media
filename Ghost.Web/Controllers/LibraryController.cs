using Ghost.Dtos;
using Ghost.Services;
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
      return libraryService.Create(library.Name);
    }

    [HttpGet]
    public ActionResult<PageResultDto<LibraryDto>> GetLibraries(int page = 0, int limit = 10)
    {
      return libraryService.GetLibraries(page, limit);
    }

    [HttpGet("{id}")]
    public ActionResult<LibraryDto> GetLibrary(int id)
    {
      try
      {
        return libraryService.GetLibrary(id);
      }
      catch (NullReferenceException)
      {
        return NotFound();
      }
    }

    [HttpPut("{id}/add-paths")]
    public ActionResult<LibraryDto> AddFoldersToLibrary(int id, [FromBody] AddPathsToLibraryDto pathsLibraryDto)
    {
      try
      {
        return libraryService.AddDirectories(id, pathsLibraryDto);
      }
      catch (NullReferenceException ex)
      {
        // return NotFound(ex.Message);
        throw ex;
      }
    }

    [HttpPut("{id}/sync")]
    public ActionResult SyncLibrary(int id)
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

    [HttpPut("{id}/sync-nfo")]
    public ActionResult SyncNfos(int id)
    {
      try
      {
        libraryService.SyncNfos(id);
        return Ok();
      }
      catch (NullReferenceException)
      {
        return NotFound();
      }
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteLibrary(int id)
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