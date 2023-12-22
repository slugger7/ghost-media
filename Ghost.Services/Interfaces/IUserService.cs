using Ghost.Dtos;

namespace Ghost.Services;
public interface IUserService
{
  UserDto FindById(int id);
  PageResultDto<UserDto> GetUsers();
  Task<UserDto> Create(CreateUserDto createUser);
  Task<UserDto> Delete(int id);
  UserDto? Login(UserLoginDto userLogin);
  Task<bool> ToggleFavouriteVideo(int id, int videoId);
  Task<bool> ToggleFavouriteActor(int id, int actorId);
}