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

        [HttpPost("login")]
        public ActionResult<UserDto> Login([FromBody] UserLoginDto userLogin) 
        {
            try 
            {
                return this.userService.Login(userLogin);
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }

        [HttpPut("{id}/video/{videoId}")]
        public async Task<ActionResult<bool>> ToggleFavouriteVideo(int id, int videoId)
        {
            return await userService.ToggleFavouriteVideo(id, videoId);
        }

        [HttpPut("{id}/actor/{actorId}")]
        public async Task<ActionResult<bool>> ToggleFavouriteActor(int id, int actorId)
        {
            return await userService.ToggleFavouriteActor(id, actorId);
        }
    }
}