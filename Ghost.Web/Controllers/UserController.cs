using Ghost.Services;
using Ghost.Dtos;
using Microsoft.AspNetCore.Mvc;
using Ghost.Exceptions;

namespace Ghost.Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class UserController : Controller
  {
    private readonly IUserService userService;
    private readonly ILogger<UserController> logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
      this.userService = userService;
      this.logger = logger;
    }

    [HttpGet]
    public ActionResult<PageResultDto<UserDto>> GetUsers()
    {
      return this.userService.GetUsers();
    }

    [HttpGet("{id}")]
    public ActionResult<UserDto> GetById(int id)
    {
      try
      {
        return this.userService.FindById(id);
      }
      catch (NullReferenceException)
      {
        return NotFound();
      }
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto createUser)
    {
      try
      {
        return await this.userService.Create(createUser);
      }
      catch (UserExisistException ex)
      {
        return BadRequest(ex.Message);
      }
    }
  }
}