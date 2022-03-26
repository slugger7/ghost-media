using Ghost.Dtos;
using Ghost.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ghost.Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class DirectoryController : Controller
  {
    private readonly IDirectoryService directoryService;
    public DirectoryController(IDirectoryService directoryService)
    {
      this.directoryService = directoryService;
    }

    [HttpGet]
    public ActionResult<List<string>> GetDirectories(string directory)
    {
      return directoryService.GetDirectories(directory);
    }
  }
}