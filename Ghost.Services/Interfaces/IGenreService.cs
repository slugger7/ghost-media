using Ghost.Dtos;

namespace Ghost.Services.Interfaces
{
  public interface IGenreService
  {
    GenreDto GetGenreByName(string name);
  }
}