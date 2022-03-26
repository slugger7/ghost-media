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

    [HttpPut("{id}/add-folder")]
    public ActionResult<LibraryDto> AddFolderToLibrary(string id, [FromBody] AddFolderToLibraryDto folderLibraryDto)
    {
      try {
        return libraryService.AddDirectoryToLibrary(id, folderLibraryDto);
      } 
      catch (NullReferenceException)
      {
        return NotFound();
      }
    }
  }
}