using Ghost.Dtos;

namespace Ghost.Services.Interfaces
{
  public interface IGenreService
  {
    GenreViewDto GetGenreByName(string name);
    PageResultDto<GenreDto> GetGenres(int page, int limit);
  }
}