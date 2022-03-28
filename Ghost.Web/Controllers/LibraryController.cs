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
      return libraryService.CreateLibrary(library.Name ?? "");
    }

    [HttpGet]
    public ActionResult<PageResultDto<LibraryDto>> GetLibraries(int page = 0, int limit = 10)
    {
      return libraryService.GetLibraries(page, limit);
    }

    [HttpPut("{id}/add-paths")]
    public ActionResult<LibraryDto> AddFolderToLibrary(string id, [FromBody] AddPathsToLibraryDto pathsLibraryDto)
    {
      try {
        return libraryService.AddDirectoryToLibrary(id, pathsLibraryDto);
      } 
      catch (NullReferenceException)
      {
        return NotFound();
      }
    }
  }
}