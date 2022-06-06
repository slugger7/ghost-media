using Ghost.Dtos;
using Ghost.Repository;

namespace Ghost.Services
{
  public class GenreService : IGenreService
  {
    private readonly IGenreRepository genreRepository;

    public GenreService(IGenreRepository genreRepository)
    {
      this.genreRepository = genreRepository;
    }

    public GenreViewDto GetGenreByName(string name)
    {
      var genre = this.genreRepository.GetByName(name);
      if (genre == null) throw new NullReferenceException("Genre not found");

      return new GenreViewDto(genre);
    }

    public List<GenreDto> GetGenres()
    {
      var genres = genreRepository.GetGenres();

      return genres.Select(g => new GenreDto(g)).ToList();
    }

    public List<GenreDto> GetGenresForVideo(int videoId)
    {
      var genres = genreRepository.GetGenresForVideo(videoId);

      return genres.Select(g => new GenreDto(g)).ToList();
    }

    public List<GenreDto> SearchTopGenres(string search, int limit = 10)
    {
      return genreRepository.Search(search, limit)
        .Select(g => new GenreDto(g))
        .ToList();
    }

    public async Task<GenreViewDto> UpdateGenre(int id, string name)
    {
      var genre = await genreRepository.UpdateGenre(id, name);

      return new GenreViewDto(genre);
    }
  }
}