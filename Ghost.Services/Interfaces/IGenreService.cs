using Ghost.Dtos;

namespace Ghost.Services
{
  public interface IGenreService
  {
    GenreViewDto GetGenreByName(string name);
    List<GenreDto> GetGenres();
    List<GenreDto> SearchTopGenres(string search, int limit = 10);
    List<GenreDto> GetGenresForVideo(int videoId);
  }
}