using Ghost.Services;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public ActionResult<List<string>> GetDirectories(string directory)
        {
            return directoryService.GetDirectories(directory);
        }
    }
}