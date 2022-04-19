using Ghost.Dtos;

namespace Ghost.Services.Interfaces
{
  public interface IGenreService
  {
    GenreViewDto GetGenreByName(string name);
    List<GenreDto> GetGenres();
    List<GenreDto> SearchTopGenres(int limit, string search);
  }
}