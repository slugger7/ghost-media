using Ghost.Services;
using Ghost.Dtos;
using Microsoft.AspNetCore.Mvc;
using Ghost.Exceptions;
using Ghost.Api.Utils;
using Microsoft.AspNetCore.Authorization;

namespace Ghost.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UserController : BaseController
{
  private readonly IUserService userService;
  private readonly ILogger<UserController> logger;

  public UserController(IUserService userService, ILogger<UserController> logger)
  {
    this.userService = userService;
    this.logger = logger;
  }

  [HttpGet]
  [Authorize]
  public ActionResult<PageResultDto<UserDto>> GetUsers()
  {
    Console.WriteLine("Getting users");
    return this.userService.GetUsers();
  }

  [HttpGet("{id}")]
  [Authorize]
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
  [AllowAnonymous]
  public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserDto createUser)
  {
    try
    {
      return await this.userService.Create(createUser);
    }
    catch (UserExistsException ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [HttpDelete("{id}")]
  [Authorize]
  public async Task<ActionResult<UserDto>> Delete(int id)
  {
    try
    {
      return await this.userService.Delete(id);
    }
    catch (NullReferenceException)
    {
      return NotFound();
    }
  }

  [AllowAnonymous]
  [HttpPost("login")]
  public ActionResult<TokenDto> Login([FromBody] UserLoginDto userLogin)
  {
    if (userLogin == null) return Unauthorized();
    string tokenString = string.Empty;
    try
    {
      var user = this.userService.Login(userLogin);

      if (user == null) return Unauthorized();

      var token = JWTAuthentication.BuildJWTToken(user.Id, user.Username);

      return new TokenDto { Token = token, UserId = user.Id };
    }
    catch (NullReferenceException)
    {
      return NotFound();
    }
  }

  [AllowAnonymous]
  [HttpGet("sym-key")]
  public ActionResult<string> GenerateSymmetricKey()
  {
    return JWTAuthentication.BuildSymKey();
  }

  [HttpPut("video/{videoId}")]
  [Authorize]
  public async Task<ActionResult<bool>> ToggleFavouriteVideo(int videoId)
  {
    return await userService.ToggleFavouriteVideo(UserId, videoId);
  }

  [HttpPut("actor/{actorId}")]
  [Authorize]
  public async Task<ActionResult<bool>> ToggleFavouriteActor(int actorId)
  {
    return await userService.ToggleFavouriteActor(UserId, actorId);
  }
}