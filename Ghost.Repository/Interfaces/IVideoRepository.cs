using Ghost.Data;
using Ghost.Dtos;

namespace Ghost.Repository;
public interface IVideoRepository
{
  Task<Video> CreateVideo(string path, VideoMetaDataDto videoMetaData, LibraryPath libraryPath, string? title = null);
  PageResult<Video> SearchVideos(int userId, string watchState, string[]? genresFilter, int page = 0, int limit = 10, string search = "", string sortBy = "title", bool ascending = true);
  PageResult<Video> GetForActor(int userId, string watchState, string[]? genresFilter, int actorId, int page = 0, int limit = 10, string search = "", string sortBy = "title", bool ascending = true);
  PageResult<Video> GetForGenre(int userId, string watchState, string[]? genresFilter, string name, int page = 0, int limit = 10, string search = "", string sortBy = "title", bool ascending = true);
  Video? FindById(int id);
  Video? FindById(int id, List<string>? includes);
  Task<Video> SetActors(int id, IEnumerable<Actor> actors);
  Task<Video> SetGenres(int id, IEnumerable<Genre> genres);
  Task<Video> Delete(int id);
  Task<Video> UpdateTitle(int id, string title);
  Task<Video> UpdateVideo(Video video);
  Task<Video> UpdateVideo(Video video, List<string>? includes);
  Task BatchUpdateFromNFO(IEnumerable<Video> videos, Dictionary<int, List<VideoGenre>> videoGenreDictionary, Dictionary<int, List<VideoActor>> videoActorDictionary);
  Task BatchUpdate(IEnumerable<Video> videos);
  Video Random(int userId, RandomVideoRequestDto randomVideoRequest);
  Video GetRandomVideoForGenre(string genre, int userId, RandomVideoRequestDto randomVideoRequest);
  Video GetRandomVideoForActor(int id, int userId, RandomVideoRequestDto randomVideoRequest);
  Task<List<Video>> RelateVideo(int id, int relateTo);
  Task<List<Video>> DeleteRelation(int id, int relatedTo);
  Task<Video> CreateSubVideo(int id, Video newVideo);
  Task<List<Video>> GetVideosByIds(IEnumerable<int> ids);
}