using Ghost.Dtos;

namespace Ghost.Services
{
  public interface IUserService
  {
    UserDto FindById(int id);
    PageResultDto<UserDto> GetUsers();
    Task<UserDto> Create(CreateUserDto createUser);
    Task<UserDto> Delete(int id);
  }
}